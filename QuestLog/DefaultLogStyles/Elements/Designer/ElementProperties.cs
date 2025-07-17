﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private static ChapterElement lastElement = null;
        private static bool previewElementInfo = false;

        private static object MemberHash(MemberInfo memberInfo) => memberInfo is FieldInfo field ? field.FieldHandle : memberInfo;

        private static readonly object[] defaultMembers = typeof(ChapterElement)
            .GetProperties().Cast<MemberInfo>().Concat(typeof(ChapterElement).GetFields().Cast<MemberInfo>())
            .Select(MemberHash).ToArray();

        private static readonly Dictionary<Type, Dictionary<MemberInfo, MemberBundle>> elementTypeMembers = [];
        private static readonly Dictionary<MemberBundle, Rectangle> memberBoxes = [];
        private static MemberInfo selectedMember = null;
        private static bool memberValueAccepted = false;
        private static int memberScrollOffset = 0;

        private class MemberBundle(MemberInfo memberInfo, string value, string tooltip, Func<string> getter, Func<string, bool> setter)
        {
            public MemberInfo MemberInfo = memberInfo;
            public string Value = value;
            public string Tooltip = tooltip;
            public Func<string> Getter = getter;
            public Func<string, bool> Setter = setter;
        }

        private void HandleElementProperties(Rectangle area, Vector2 mousePosition)
        {
            Point mouseCanvas = mousePosition.ToPoint();

            if (previewElementInfo)
            {
                DrawTasks.Add(SelectedElement.DrawInfoPage);
                goto DrawPreviewButton;
            }

            Rectangle elementProperties = area.CookieCutter(new(0f, 0.05f), new(0.95f, 0.8f));
            Rectangle deleteElement = elementProperties.CookieCutter(new(-0.9f, 1.075f), new(0.1f, 0.06f));
            Rectangle propertyTitle = elementProperties.CookieCutter(new(-0.13f, -1.06f), new(0.85f, 0.1f));
            bool deleteElementHovered = false;

            if (deleteElement.Contains(mouseCanvas))
            {
                deleteElementHovered = true;
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.DeleteElement");

                if (LeftMouseJustReleased)
                {
                    // Display a pop up to make sure the user wants to delete the book
                    SDL.SDL_MessageBoxData message = new()
                    {
                        window = Main.instance.Window.Handle,
                        title = "Delete Element",
                        message = $"Are you sure you want to delete the selected element: {SelectedElement.GetType().Name}?\n" +
                        "This action cannot be undone!",
                        flags = SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_WARNING,
                        numbuttons = 2,
                        buttons = [
                            new SDL.SDL_MessageBoxButtonData()
                            {
                                buttonid = 2,
                                flags = SDL.SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_ESCAPEKEY_DEFAULT,
                                text = "Cancel"
                            },
                            new SDL.SDL_MessageBoxButtonData()
                            {
                                buttonid = 1,
                                flags = SDL.SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_RETURNKEY_DEFAULT,
                                text = "Delete"
                            }
                        ]
                    };

                    SoundEngine.PlaySound(SoundID.MenuOpen);
                    int result = SDL.SDL_ShowMessageBox(ref message, out int buttonId);

                    // If okay...
                    if (result == 0 && buttonId == 1)
                    {
                        SelectedChapter.Elements.Remove(SelectedElement);
                        SelectedElement.OnDelete();
                        SortedElements = null;
                        SelectedElement = null;
                        questInfoSwipeOffset = -questInfoTarget.Height;
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        return;
                    }

                    else
                        SoundEngine.PlaySound(SoundID.MenuClose);
                }
            }

            DrawTasks.Add(sb =>
            {
                Texture2D texture = deleteElementHovered ? QuestAssets.DeleteButtonHovered : QuestAssets.DeleteButton;
                sb.Draw(texture, deleteElement.Center(), null, Color.White, 0f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
            });

            //AddRectangle(elementProperties, Color.Red);            
            DrawTasks.Add(sb => sb.DrawOutlinedStringInRectangle(propertyTitle, FontAssets.DeathText.Value, Color.White, Color.Black, "Element Properties:", alignment: Utilities.TextAlignment.Left, clipBounds: false));

            // Alright, there's a lot going on here...
            var elementType = SelectedElement.GetType();
            Dictionary<MemberInfo, MemberBundle> members = [];

            if (!elementTypeMembers.TryGetValue(elementType, out members))
            {
                // Set up a collection for this element type's members
                elementTypeMembers.Add(elementType, []);
                var ignoredAttribute = typeof(ChapterElement.HideInDesignerAttribute);

                // Get all public instanced properties and fields
                var properties = elementType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                var fields = elementType.GetFields(BindingFlags.Instance | BindingFlags.Public);

                // Get all members not implemented by the default ChapterElement class
                IEnumerable<MemberInfo> memberInfos = properties.Where(x => x.CanWrite).Cast<MemberInfo>().Concat(fields.Cast<MemberInfo>())
                    .Where(x => !defaultMembers.Contains(MemberHash(x)))
                    .Where(x => Attribute.GetCustomAttribute(x, ignoredAttribute) is null);

                foreach (var memberInfo in memberInfos)
                {
                    // Check if the element has been tagged to use a custom converter
                    ChapterElement.UseConverterAttribute attribute = Attribute
                        .GetCustomAttribute(memberInfo, typeof(ChapterElement.UseConverterAttribute))
                        as ChapterElement.UseConverterAttribute;

                    // These act as simplified get-set methods regardless of whether
                    // the member is a field or property
                    Func<object> getter;
                    Action<object> setter;
                    Type memberType;

                    if (memberInfo is FieldInfo field)
                    {
                        getter = () => field.GetValue(SelectedElement);
                        setter = (value) => field.SetValue(SelectedElement, value);
                        memberType = field.FieldType;
                    }
                    else
                    {
                        PropertyInfo property = memberInfo as PropertyInfo;
                        getter = () => property.GetValue(SelectedElement);
                        setter = (value) => property.SetValue(SelectedElement, value);
                        memberType = property.PropertyType;
                    }

                    // First check for an attributed converter,
                    // then check if we have a default converter for the type
                    var converterType = attribute is not null && attribute.PropertyConverterType.IsAssignableTo(
                        typeof(ChapterElement.IMemberConverter<>).MakeGenericType(memberType)) ?
                        attribute.PropertyConverterType :
                        ChapterElement.DefaultConverters.GetValueOrDefault(memberType, null);

                    // If no converter works, skip over
                    if (converterType is null)
                        continue;

                    // Get the converter
                    var converter = Activator.CreateInstance(converterType);

                    // Get the conversion methods
                    MethodInfo toString = converterType.GetMethod("Convert", BindingFlags.Instance | BindingFlags.Public, [memberType]);
                    MethodInfo fromString = converterType.GetMethod("TryParse", BindingFlags.Instance | BindingFlags.Public, [typeof(string), memberType.MakeByRefType()]);

                    // The getter is used to initially populate fields
                    string elementGetter() => (string)toString.Invoke(converter, [getter()]);

                    // The setter wraps in a TryParse so that we know whether it worked
                    bool elementSetter(string value)
                    {
                        object[] param = [value, default];

                        if ((bool)fromString.Invoke(converter, param))
                        {
                            setter(param[1]);
                            return true;
                        }

                        return false;
                    }

                    string tooltipKey = Attribute.GetCustomAttribute(memberInfo, typeof(TooltipAttribute)) is TooltipAttribute tooltip ?
                        tooltip.LocalizationKey :
                        null;

                    // Set up the member modification methods
                    elementTypeMembers[elementType].Add(memberInfo, new(memberInfo, "", tooltipKey, elementGetter, elementSetter));
                }

                // Re-get the members now that they've been added
                members = elementTypeMembers[elementType];
            }

            // Re-populate values if switching elements and reset the scroll position
            if (SelectedElement != lastElement)
            {
                foreach (MemberInfo memberInfo in members.Keys)
                    members[memberInfo].Value = members[memberInfo].Getter();

                memberScrollOffset = 0;
            }

            // Clear the current boxes
            lastElement = SelectedElement;
            memberBoxes.Clear();

            // Add boxes for each modifiable member
            Rectangle memberBox = elementProperties.CreateScaledMargin(0.01f).CookieCutter(new(0f, -0.88f), new(1f, 0.1f));
            foreach (MemberInfo member in members.Keys)
            {
                memberBoxes.Add(members[member], memberBox);
                memberBox = memberBox.CookieCutter(new(0f, 2.25f), Vector2.One);
            }

            // Scroll if applicable
            if (elementProperties.Contains(mouseCanvas))
            {
                int data = PlayerInput.ScrollWheelDeltaForUI;

                if (data != 0)
                {
                    int scrollAmount = data / 6;
                    int initialOffset = memberScrollOffset;
                    memberScrollOffset += scrollAmount;

                    Rectangle lastBox = memberBoxes.Values.Last();
                    int minScrollValue = -(lastBox.Bottom - (elementProperties.Height + elementProperties.Y));

                    memberScrollOffset = minScrollValue < 0 ? int.Clamp(memberScrollOffset, minScrollValue, 0) : 0;
                }
            }

            DrawTasks.Add(sb =>
            {
                sb.End();
                sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);

                // We use a transformation matrix here so we need to be careful
                Rectangle scissor = new(
                    (int)(elementProperties.X * TargetScale),
                    (int)(elementProperties.Y * TargetScale),
                    (int)(elementProperties.Width * TargetScale),
                    (int)(elementProperties.Height * TargetScale));

                // Scissor out the member area
                sb.GraphicsDevice.ScissorRectangle = scissor;
                raster.ScissorTestEnable = true;

                sb.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
            });

            // Used for selected members to flash red/yellow
            float colorLerp = (float)(Main.timeForVisualEffects % 60);

            if (colorLerp > 30)
                colorLerp -= (colorLerp % 30) * 2;

            colorLerp /= 30f;

            // Return on enter or escape
            if (selectedMember is not null)
            {
                CancelChat = true;
                if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) || Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                    selectedMember = null;
            }

            foreach (var (bundle, box) in memberBoxes)
            {
                // Apply scroll offset
                box.Offset(0, memberScrollOffset);

                // Designated area for value and name
                Rectangle typeArea = box.CookieCutter(new(0.3f, 0f), new(0.7f, 1f));
                Rectangle memberArea = box.CookieCutter(new(-0.72f, 0f), new(0.28f, 1f));

                // Draw the name
                AddRectangle(typeArea, Color.Gray * 0.6f, fill: true);
                DrawTasks.Add(sb => sb.DrawOutlinedStringInRectangle(
                    memberArea.CookieCutter(new(0f, 0.25f), Vector2.One),
                    FontAssets.DeathText.Value,
                    Color.White, Color.Black,
                    $"{bundle.MemberInfo.Name}:",
                    alignment: Utilities.TextAlignment.Left,
                    clipBounds: false));

                // Self explanatory
                bool selected = bundle.MemberInfo == selectedMember;

                if (memberArea.Contains(mouseCanvas))
                {
                    if (bundle.Tooltip is not null)
                        MouseTooltip = Language.GetTextValue(bundle.Tooltip);
                }

                if (typeArea.Contains(mouseCanvas))
                {
                    // If we clicked a new member, start editing it
                    // If we clicked the same one, stop editing it
                    if (LeftMouseJustReleased)
                    {
                        MemberInfo member = selected ? null : bundle.MemberInfo;
                        selectedMember = member;
                        memberValueAccepted = true;
                        selected = member is not null;
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                }

                // If we were editing and clicked somewhere else, stop editing
                else if (selected && LeftMouseJustReleased)
                {
                    selectedMember = null;
                    selected = false;
                }

                // Modify the selected member
                if (selected)
                {
                    DrawTasks.Add(_ =>
                     {
                         PlayerInput.WritingText = true;
                         Main.instance.HandleIME();
                     });

                    string value = bundle.Value;
                    string newValue = Main.GetInputText(value);

                    // Only do parsing on new values (every frame would be ridiculous)
                    if (value != newValue)
                    {
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        memberValueAccepted = bundle.Setter(newValue);
                        bundle.Value = newValue;
                    }

                    // Yellow for OK, red for invalid
                    Color boxColor = memberValueAccepted ? Color.Yellow : Color.Red;
                    AddRectangle(typeArea, Color.Lerp(Color.Black, boxColor, colorLerp));
                }

                // Hovering
                else if (typeArea.Contains(mouseCanvas))
                    AddRectangle(typeArea, Color.LightGray);

                // Standard
                else
                    AddRectangle(typeArea, Color.Black);

                // Draw the element value
                DrawTasks.Add(sb => sb.DrawOutlinedStringInRectangle(
                    typeArea.CreateScaledMargin(0.01f).CookieCutter(new(0f, 0.25f), Vector2.One),
                    FontAssets.DeathText.Value,
                    Color.White, Color.Black,
                    bundle.Value,
                    clipBounds: true,
                    minimumScale: 0.4f,
                    alignment: Utilities.TextAlignment.Right,
                    offset: new(-2f, 0f)));
            }

            DrawTasks.Add(sb =>
            {
                sb.End();
                sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);

                // Undo our rectangle
                sb.GraphicsDevice.ScissorRectangle = new(0, 0, Main.screenWidth, Main.screenHeight);
                raster.ScissorTestEnable = false;

                sb.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
            });

        DrawPreviewButton:

            Rectangle previewToggle = area.CookieCutter(new(0.82f, -0.85f), new(0.125f, 0.075f));
            AddRectangle(previewToggle, Color.Blue, fill: true);

            if (previewElementInfo)
                AddRectangle(previewToggle, Color.White);

            if (previewToggle.Contains(mouseCanvas))
            {
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.TogglePreview");

                if (LeftMouseJustReleased)
                    previewElementInfo = !previewElementInfo;
            }
        }
    }
}

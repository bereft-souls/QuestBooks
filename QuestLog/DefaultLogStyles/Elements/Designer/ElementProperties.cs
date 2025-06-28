using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Systems;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

        private class MemberBundle(MemberInfo memberInfo, string value, Func<string> getter, Func<string, bool> setter)
        {
            public MemberInfo MemberInfo = memberInfo;
            public string Value = value;
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
                MouseTooltip = "Delete Selected Element";

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

            var elementType = SelectedElement.GetType();
            Dictionary<MemberInfo, MemberBundle> members = [];

            if (!elementTypeMembers.TryGetValue(elementType, out members))
            {
                elementTypeMembers.Add(elementType, []);
                var ignoredAttribute = typeof(ChapterElement.HideInDesignerAttribute);

                var properties = elementType.GetProperties();
                var fields = elementType.GetFields();

                IEnumerable<MemberInfo> memberInfos = properties.Where(x => x.CanWrite).Cast<MemberInfo>().Concat(fields.Cast<MemberInfo>())
                    .Where(x => !defaultMembers.Contains(MemberHash(x)))
                    .Where(x => Attribute.GetCustomAttribute(x, ignoredAttribute) is null);

                foreach (var memberInfo in memberInfos)
                {
                    ChapterElement.UseCustomConverterAttribute attribute = Attribute
                        .GetCustomAttribute(memberInfo, typeof(ChapterElement.UseCustomConverterAttribute))
                        as ChapterElement.UseCustomConverterAttribute;

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

                    var converterType = attribute is not null && attribute.PropertyConverterType.IsAssignableTo(
                        typeof(ChapterElement.IPropertyConverter<>).MakeGenericType(memberType)) ?
                        attribute.PropertyConverterType :
                        ChapterElement.DefaultConverters.GetValueOrDefault(memberType, null);

                    if (converterType is null)
                        continue;

                    var converter = Activator.CreateInstance(converterType);

                    MethodInfo toString = converterType.GetMethod("Convert", BindingFlags.Instance | BindingFlags.Public, [memberType]);
                    MethodInfo fromString = converterType.GetMethod("TryParse", BindingFlags.Instance | BindingFlags.Public, [typeof(string), memberType.MakeByRefType()]);

                    string elementGetter() => (string)toString.Invoke(converter, [getter()]);
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

                    elementTypeMembers[elementType].Add(memberInfo, new(memberInfo, "", elementGetter, elementSetter));
                }

                members = elementTypeMembers[elementType];
            }

            if (SelectedElement != lastElement)
            {
                foreach (MemberInfo memberInfo in members.Keys)
                    members[memberInfo].Value = members[memberInfo].Getter();

                memberScrollOffset = 0;
            }

            lastElement = SelectedElement;
            memberBoxes.Clear();

            Rectangle memberBox = elementProperties.CreateScaledMargin(0.01f).CookieCutter(new(0f, -0.88f), new(1f, 0.1f));

            foreach (MemberInfo member in members.Keys)
            {
                memberBoxes.Add(members[member], memberBox);
                memberBox = memberBox.CookieCutter(new(0f, 2.25f), Vector2.One);
            }

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

                Rectangle scissor = new(
                    (int)(elementProperties.X * TargetScale),
                    (int)(elementProperties.Y * TargetScale),
                    (int)(elementProperties.Width * TargetScale),
                    (int)(elementProperties.Height * TargetScale));

                sb.GraphicsDevice.ScissorRectangle = scissor;
                raster.ScissorTestEnable = true;

                sb.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
            });

            float colorLerp = (float)(Main.timeForVisualEffects % 60);

            if (colorLerp > 30)
                colorLerp -= (colorLerp % 30) * 2;

            colorLerp /= 30f;

            foreach (var (bundle, box) in memberBoxes)
            {
                box.Offset(0, memberScrollOffset);

                Rectangle typeArea = box.CookieCutter(new(0.3f, 0f), new(0.7f, 1f));
                Rectangle memberArea = box.CookieCutter(new(-0.7f, 0f), new(0.3f, 1f));

                AddRectangle(typeArea, Color.Gray * 0.6f, fill: true);
                DrawTasks.Add(sb => sb.DrawOutlinedStringInRectangle(
                    memberArea.CookieCutter(new(0f, 0.25f), Vector2.One),
                    FontAssets.DeathText.Value,
                    Color.White, Color.Black,
                    $"{bundle.MemberInfo.Name}:",
                    alignment: Utilities.TextAlignment.Left,
                    clipBounds: false));

                bool selected = bundle.MemberInfo == selectedMember;

                if (box.Contains(mouseCanvas))
                {
                    if (LeftMouseJustReleased)
                    {
                        MemberInfo member = selected ? null : bundle.MemberInfo;
                        selectedMember = member;
                        memberValueAccepted = true;
                        selected = member is not null;
                    }
                }

                else if (selected && LeftMouseJustReleased)
                {
                    selectedMember = null;
                    selected = false;
                }

                if (selected)
                {
                   DrawTasks.Add(_ =>
                    {
                        PlayerInput.WritingText = true;
                        Main.instance.HandleIME();
                    });

                    string value = bundle.Value;
                    string newValue = Main.GetInputText(value);

                    if (value != newValue)
                    {
                        memberValueAccepted = bundle.Setter(newValue);
                        bundle.Value = newValue;
                    }

                    Color boxColor = memberValueAccepted ? Color.Yellow : Color.Red;
                    AddRectangle(typeArea, Color.Lerp(Color.Black, boxColor, colorLerp));
                }

                else if (box.Contains(mouseCanvas))
                    AddRectangle(typeArea, Color.LightGray);

                else
                    AddRectangle(typeArea, Color.Black);

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
                MouseTooltip = "Toggle Preview";

                if (LeftMouseJustReleased)
                    previewElementInfo = !previewElementInfo;
            }
        }
    }
}

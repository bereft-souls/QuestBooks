using Microsoft.Win32.SafeHandles;
using Microsoft.Xna.Framework;
using QuestBooks.QuestLog.DefaultQuestBooks;
using QuestBooks.QuestLog.DefaultQuestLines;
using QuestBooks.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultQuestLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private static readonly List<(Rectangle area, Action onClick, bool selected, Type type)> TypeSelections = [];

        private static bool SelectingBookType = false;
        private static bool SelectingLineType = false;

        private static int BookTypeScrollOffset = 0;
        private static int LineTypeScrollOffset = 0;

        private void HandleTypeSelection()
        {
            Rectangle questBookType = LogArea.CookieCutter(new(-1.28f, -0.815f), new(0.15f, 0.05f));
            Rectangle questLineType = questBookType.CookieCutter(new(0f, 5f), Vector2.One);
            Rectangle typeDropDown = LogArea.CookieCutter(new(-1.22f, 0.26f), new(0.21f, 0.74f));

            if (SelectedBook is not null)
            {
                AddRectangle(questBookType, Color.Gray * 0.6f, fill: true);
                AddRectangle(questBookType, SelectingBookType ? Color.Yellow : (questBookType.Contains(MouseCanvas) ? Color.LightGray : Color.Black), 3f);

                var font = FontAssets.DeathText.Value;
                string typeName = SelectedBook.GetType().Name;

                DrawTasks.Add(sb =>
                {
                    sb.DrawOutlinedStringInRectangle(questBookType.CookieCutter(new(0f, -1.6f), Vector2.One), font, Color.White, Color.Black, Language.GetTextValue("Mods.QuestBooks.Tooltips.QuestBookType"), maxScale: 0.5f);
                    questBookType = questBookType.CookieCutter(new(0f, 0.2f), Vector2.One);
                    sb.DrawOutlinedStringInRectangle(questBookType.CreateScaledMargin(0.02f).CreateScaledMargins(top: 0.0f), font, Color.White, Color.Black, typeName, 2f, alignment: Utilities.TextAlignment.Left);
                });

                if (questBookType.Contains(MouseCanvas))
                {
                    LockMouse();
                    MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ChangeQuestBookType");

                    if (LeftMouseJustReleased)
                    {
                        SelectingBookType = !SelectingBookType;
                        SelectingLineType = false;
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                }
            }

            else
                SelectingBookType = false;

            if (SelectedChapter is not null && (SelectedBook?.QuestLines.Contains(SelectedChapter) ?? false))
            {
                AddRectangle(questLineType, Color.Gray * 0.6f, fill: true);
                AddRectangle(questLineType, SelectingLineType ? Color.Yellow : (questLineType.Contains(MouseCanvas) ? Color.LightGray : Color.Black), 3f);

                var font = FontAssets.DeathText.Value;
                string typeName = SelectedChapter.GetType().Name;

                DrawTasks.Add(sb =>
                {
                    sb.DrawOutlinedStringInRectangle(questLineType.CookieCutter(new(0f, -1.6f), Vector2.One), font, Color.White, Color.Black, Language.GetTextValue("Mods.QuestBooks.Tooltips.QuestLineType"), maxScale: 0.5f);
                    questLineType = questLineType.CookieCutter(new(0f, 0.2f), Vector2.One);
                    sb.DrawOutlinedStringInRectangle(questLineType.CreateScaledMargin(0.02f).CreateScaledMargins(top: 0.0f), font, Color.White, Color.Black, typeName, 2f, alignment: Utilities.TextAlignment.Left);
                });

                if (questLineType.Contains(MouseCanvas))
                {
                    LockMouse();
                    MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ChangeQuestLineType");

                    if (LeftMouseJustReleased)
                    {
                        SelectingLineType = !SelectingLineType;
                        SelectingBookType = false;
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                }
            }

            else
                SelectingLineType = false;

            if (SelectingBookType || SelectingLineType)
            {
                AddRectangle(typeDropDown, Color.Gray * 0.6f, fill: true);
                AddRectangle(typeDropDown, Color.Black, stroke: 3f);
                TypeSelections.Clear();

                Rectangle typeBox = typeDropDown.CreateScaledMargin(0.025f).CookieCutter(new(0f, -0.95f), new(1f, 0.078f));
                if (SelectingBookType)
                {
                    foreach (Type bookType in AvailableQuestBookTypes)
                    {
                        void onClick()
                        {
                            var instance = (BasicQuestBook)Activator.CreateInstance(bookType);
                            SelectedBook.CloneTo(instance);
                            QuestManager.QuestBooks[QuestManager.QuestBooks.IndexOf(SelectedBook)] = instance;
                            SelectedBook = instance;
                        }

                        TypeSelections.Add((typeBox, onClick, bookType == SelectedBook.GetType(), bookType));
                        typeBox = typeBox.CookieCutter(new(0, 2.2f), Vector2.One);
                    }
                }

                else
                {
                    foreach (Type lineType in AvailableQuestLineTypes)
                    {
                        void onClick()
                        {
                            var instance = (BasicQuestLine)Activator.CreateInstance(lineType);
                            SelectedChapter.CloneTo(instance);
                            SelectedBook.QuestLines[SelectedBook.QuestLines.IndexOf(SelectedChapter)] = instance;
                            SelectedChapter = instance;
                        }

                        TypeSelections.Add((typeBox, onClick, lineType == SelectedChapter.GetType(), lineType));
                        typeBox = typeBox.CookieCutter(new(0, 2.2f), Vector2.One);
                    }
                }

                if (typeDropDown.Contains(MouseCanvas))
                {
                    LockMouse();
                    int data = PlayerInput.ScrollWheelDeltaForUI;

                    if (data != 0)
                    {
                        int scrollAmount = data / 6;
                        if (SelectingBookType)
                            UpdateScroll(ref BookTypeScrollOffset);
                        else
                            UpdateScroll(ref LineTypeScrollOffset);

                        void UpdateScroll(ref int scrollOffset)
                        {
                            int initialOffset = scrollOffset;
                            scrollOffset += scrollAmount;

                            Rectangle lastBox = TypeSelections[^1].area;
                            int minScrollValue = -(lastBox.Bottom - (typeDropDown.Height + typeDropDown.Y));

                            scrollOffset = minScrollValue < 0 ? int.Clamp(scrollOffset, minScrollValue, 0) : 0;
                        }
                    }
                }

                DrawTasks.Add(sb =>
                {
                    sb.End();
                    sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);

                    sb.GraphicsDevice.ScissorRectangle = typeDropDown;
                    raster.ScissorTestEnable = true;
                    
                    sb.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
                });

                foreach (var (box, onClick, selected, type) in TypeSelections)
                {
                    int offset = SelectingBookType ? BookTypeScrollOffset : LineTypeScrollOffset;
                    box.Offset(0, offset);
                    AddRectangle(box, Color.Gray, fill: true);

                    if (selected)
                        AddRectangle(box, Color.Yellow);

                    DrawTasks.Add(sb => sb.DrawOutlinedStringInRectangle(box.CookieCutter(new(0f, 0.3f), Vector2.One), FontAssets.DeathText.Value, Color.White, Color.Black, type.Name));

                    if (box.Contains(MouseCanvas) && typeDropDown.Contains(MouseCanvas))
                    {
                        if (!selected)
                            AddRectangle(box, Color.White);

                        MouseTooltip = type.FullName;

                        if (LeftMouseJustReleased)
                        {
                            onClick();
                            SoundEngine.PlaySound(SoundID.MenuTick);
                        }
                    }
                }

                DrawTasks.Add(sb =>
                {
                    sb.End();
                    sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);

                    sb.GraphicsDevice.ScissorRectangle = new(0, 0, Main.screenWidth, Main.screenHeight);
                    raster.ScissorTestEnable = false;

                    sb.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
                });
            }
        }
    }
}

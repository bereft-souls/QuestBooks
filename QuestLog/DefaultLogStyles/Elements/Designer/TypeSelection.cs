using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Systems;
using System;
using System.Collections.Generic;
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
        private readonly List<(Rectangle area, Action onClick, bool selected, Type type)> typeSelections = [];

        private bool selectingBookType = false;
        private bool selectingLineType = false;

        private int bookTypeScrollOffset = 0;
        private int lineTypeScrollOffset = 0;

        private void HandleTypeSelection()
        {
            Rectangle questBookType = LogArea.CookieCutter(new(-1.28f, -0.82f), new(0.15f, 0.05f));
            Rectangle questLineType = questBookType.CookieCutter(new(0f, 5f), Vector2.One);
            Rectangle typeDropDown = LogArea.CookieCutter(new(-1.22f, 0.26f), new(0.21f, 0.74f));

            Rectangle bookMovement = LogArea.CookieCutter(new(-0.955f, -0.1f), new(0.02f, 0.063f));
            Rectangle bookUp = bookMovement.CookieCutter(new(0f, -0.5f), new(1f, 0.5f));
            Rectangle bookDown = bookUp.CookieCutter(new(0f, 2f), Vector2.One);

            Rectangle chapterMovement = LogArea.CookieCutter(new(-0.05f, -0.1f), new(0.02f, 0.063f));
            Rectangle chapterUp = chapterMovement.CookieCutter(new(0f, -0.5f), new(1f, 0.5f));
            Rectangle chapterDown = chapterUp.CookieCutter(new(0f, 2f), Vector2.One);

            if (SelectedBook is not null)
            {
                if (SelectedElement is null)
                {
                    int bookIndex = QuestManager.QuestBooks.IndexOf(SelectedBook);
                    bool firstBook = bookIndex == 0;
                    bool lastBook = bookIndex == QuestManager.QuestBooks.Count - 1;

                    bool bookUpHovered = false;
                    bool bookDownHovered = false;

                    if (bookUp.Contains(MouseCanvas))
                    {
                        MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ShiftBookUp");

                        if (LeftMouseJustReleased && !firstBook)
                            (QuestManager.QuestBooks[bookIndex], QuestManager.QuestBooks[bookIndex - 1]) = (QuestManager.QuestBooks[bookIndex - 1], QuestManager.QuestBooks[bookIndex]);
                    }

                    else if (bookDown.Contains(MouseCanvas))
                    {
                        MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ShiftBookDown");

                        if (LeftMouseJustReleased && !lastBook)
                            (QuestManager.QuestBooks[bookIndex], QuestManager.QuestBooks[bookIndex + 1]) = (QuestManager.QuestBooks[bookIndex + 1], QuestManager.QuestBooks[bookIndex]);
                    }

                    Texture2D bookUpTexture = bookUpHovered && !firstBook ? QuestAssets.ShiftBookUpHovered : QuestAssets.ShiftBookUp;
                    Texture2D bookDownTexture = bookDownHovered && !lastBook ? QuestAssets.ShiftBookDownHovered : QuestAssets.ShiftBookDown;

                    DrawTasks.Add(sb =>
                    {
                        float scale = 0.5f * LogScale;
                        sb.Draw(bookUpTexture, bookUp.Center(), null, Color.White * (firstBook ? 0.4f : 1f), 0f, bookUpTexture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                        sb.Draw(bookDownTexture, bookDown.Center(), null, Color.White * (lastBook ? 0.4f : 1f), 0f, bookDownTexture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                    });
                }

                AddRectangle(questBookType, Color.Gray * 0.6f, fill: true);
                AddRectangle(questBookType, selectingBookType ? Color.Yellow : (questBookType.Contains(MouseCanvas) ? Color.LightGray : Color.Black), 3f);

                var font = FontAssets.DeathText.Value;
                string typeName = SelectedBook.GetType().Name;

                DrawTasks.Add(sb =>
                {
                    sb.DrawOutlinedStringInRectangle(questBookType.CookieCutter(new(0f, -1.6f), Vector2.One), font, Color.White, Color.Black, Language.GetTextValue("Mods.QuestBooks.Tooltips.QuestBookType"), maxScale: 0.5f);
                    questBookType = questBookType.CookieCutter(new(0f, 0.2f), Vector2.One);
                    sb.DrawOutlinedStringInRectangle(questBookType.CreateMargins(left: 2, right: 2), font, Color.White, Color.Black, typeName, 2f, alignment: Utilities.TextAlignment.Left);
                });

                if (questBookType.Contains(MouseCanvas))
                {
                    LockMouse();
                    MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ChangeQuestBookType");

                    if (LeftMouseJustReleased)
                    {
                        selectingBookType = !selectingBookType;
                        selectingLineType = false;
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                }
            }

            else
                selectingBookType = false;

            if (SelectedChapter is not null && (SelectedBook?.Chapters.Contains(SelectedChapter) ?? false))
            {
                if (SelectedElement is null)
                {
                    int chapterIndex = SelectedBook.Chapters.IndexOf(SelectedChapter);
                    bool firstChapter = chapterIndex == 0;
                    bool lastChapter = chapterIndex == SelectedBook.Chapters.Count - 1;

                    bool chapterUpHovered = false;
                    bool chapterDownHovered = false;

                    if (chapterUp.Contains(MouseCanvas))
                    {
                        MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ShiftChapterUp");
                        chapterUpHovered = true;

                        if (LeftMouseJustReleased && !firstChapter)
                            (SelectedBook.Chapters[chapterIndex], SelectedBook.Chapters[chapterIndex - 1]) = (SelectedBook.Chapters[chapterIndex - 1], SelectedBook.Chapters[chapterIndex]);
                    }

                    else if (chapterDown.Contains(MouseCanvas))
                    {
                        MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ShiftChapterDown");
                        chapterDownHovered = true;

                        if (LeftMouseJustReleased && !lastChapter)
                            (SelectedBook.Chapters[chapterIndex], SelectedBook.Chapters[chapterIndex + 1]) = (SelectedBook.Chapters[chapterIndex + 1], SelectedBook.Chapters[chapterIndex]);
                    }

                    Texture2D chapterUpTexture = chapterUpHovered && !firstChapter ? QuestAssets.ShiftBookUpHovered : QuestAssets.ShiftBookUp;
                    Texture2D chapterDownTexture = chapterDownHovered && !lastChapter ? QuestAssets.ShiftBookDownHovered : QuestAssets.ShiftBookDown;

                    DrawTasks.Add(sb =>
                    {
                        float scale = 0.5f * LogScale;
                        sb.Draw(chapterUpTexture, chapterUp.Center(), null, Color.White * (firstChapter ? 0.4f : 1f), 0f, chapterUpTexture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                        sb.Draw(chapterDownTexture, chapterDown.Center(), null, Color.White * (lastChapter ? 0.4f : 1f), 0f, chapterDownTexture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                    });
                }

                AddRectangle(questLineType, Color.Gray * 0.6f, fill: true);
                AddRectangle(questLineType, selectingLineType ? Color.Yellow : (questLineType.Contains(MouseCanvas) ? Color.LightGray : Color.Black), 3f);

                var font = FontAssets.DeathText.Value;
                string typeName = SelectedChapter.GetType().Name;

                DrawTasks.Add(sb =>
                {
                    sb.DrawOutlinedStringInRectangle(questLineType.CookieCutter(new(0f, -1.6f), Vector2.One), font, Color.White, Color.Black, Language.GetTextValue("Mods.QuestBooks.Tooltips.QuestLineType"), maxScale: 0.5f);
                    questLineType = questLineType.CookieCutter(new(0f, 0.2f), Vector2.One);
                    sb.DrawOutlinedStringInRectangle(questLineType.CreateMargins(left: 2, right: 2), font, Color.White, Color.Black, typeName, 2f, alignment: Utilities.TextAlignment.Left);
                });

                if (questLineType.Contains(MouseCanvas))
                {
                    LockMouse();
                    MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ChangeChapterType");

                    if (LeftMouseJustReleased)
                    {
                        selectingLineType = !selectingLineType;
                        selectingBookType = false;
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                }
            }

            else
                selectingLineType = false;

            if (selectingBookType || selectingLineType)
            {
                AddRectangle(typeDropDown, Color.Gray * 0.6f, fill: true);
                AddRectangle(typeDropDown, Color.Black, stroke: 3f);
                typeSelections.Clear();

                Rectangle typeBox = typeDropDown.CreateScaledMargin(0.025f).CookieCutter(new(0f, -0.95f), new(1f, 0.078f));
                if (selectingBookType)
                {
                    foreach (Type bookType in QuestManager.AvailableQuestBookTypes)
                    {
                        void onClick()
                        {
                            var instance = (QuestBook)Activator.CreateInstance(bookType);
                            SelectedBook.CloneTo(instance);
                            QuestManager.QuestBooks[QuestManager.QuestBooks.IndexOf(SelectedBook)] = instance;
                            SelectedBook = instance;
                        }

                        typeSelections.Add((typeBox, onClick, bookType == SelectedBook.GetType(), bookType));
                        typeBox = typeBox.CookieCutter(new(0, 2.2f), Vector2.One);
                    }
                }

                else
                {
                    foreach (Type lineType in QuestManager.AvailableQuestLineTypes)
                    {
                        void onClick()
                        {
                            var instance = (BookChapter)Activator.CreateInstance(lineType);
                            SelectedChapter.CloneTo(instance);
                            SelectedBook.Chapters[SelectedBook.Chapters.IndexOf(SelectedChapter)] = instance;
                            SelectedChapter = instance;
                        }

                        typeSelections.Add((typeBox, onClick, lineType == SelectedChapter.GetType(), lineType));
                        typeBox = typeBox.CookieCutter(new(0f, 2.2f), Vector2.One);
                    }
                }

                if (typeDropDown.Contains(MouseCanvas))
                {
                    LockMouse();
                    int data = PlayerInput.ScrollWheelDeltaForUI;

                    if (data != 0)
                    {
                        int scrollAmount = data / 6;
                        if (selectingBookType)
                            UpdateScroll(ref bookTypeScrollOffset);
                        else
                            UpdateScroll(ref lineTypeScrollOffset);

                        void UpdateScroll(ref int scrollOffset)
                        {
                            int initialOffset = scrollOffset;
                            scrollOffset += scrollAmount;

                            Rectangle lastBox = typeSelections[^1].area;
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

                foreach (var (box, onClick, selected, type) in typeSelections)
                {
                    int offset = selectingBookType ? bookTypeScrollOffset : lineTypeScrollOffset;
                    box.Offset(0, offset);
                    AddRectangle(box, Color.Gray, fill: true);

                    if (selected)
                        AddRectangle(box, Color.Yellow);

                    else
                        AddRectangle(box, Color.LightGray);

                    DrawTasks.Add(sb => sb.DrawOutlinedStringInRectangle(box.CookieCutter(new(0f, 0.3f), Vector2.One), FontAssets.DeathText.Value, Color.White, Color.Black, type.Name));

                    if (box.Contains(MouseCanvas) && typeDropDown.Contains(MouseCanvas))
                    {
                        if (!selected)
                            AddRectangle(box, Color.White);

                        MouseTooltip = $"[c/CCC018:{type.FullName}]";

                        if (Attribute.GetCustomAttribute(type, typeof(TooltipAttribute)) is TooltipAttribute tooltip)
                            MouseTooltip += $"\n{Language.GetTextValue(tooltip.LocalizationKey)}";

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

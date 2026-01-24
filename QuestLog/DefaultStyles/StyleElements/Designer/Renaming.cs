using Microsoft.Xna.Framework;
using QuestBooks.QuestLog.DefaultChapters;
using QuestBooks.QuestLog.DefaultQuestBooks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultStyles
{
    public partial class BasicQuestLogStyle
    {
        private bool typingBookName = false;
        private bool typingChapterName = false;

        private string lastBookName = null;
        private string lastChapterName = null;

        private void HandleRenaming(Rectangle books, Rectangle chapters, Rectangle questArea)
        {
            Rectangle bookNameArea = books.CookieCutter(new(-0.15f, -1.25f), new(1.15f, 0.08f));
            Rectangle chapterNameArea = chapters.CookieCutter(new(0.15f, -1.25f), new(1.15f, 0.08f));

            float colorLerp = (float)(Main.timeForVisualEffects % 60);

            if (colorLerp > 30)
                colorLerp -= (colorLerp % 30) * 2;

            colorLerp /= 30f;
            bool saveNames = false;

            if (typingBookName || typingChapterName)
            {
                CancelChat = true;

                if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) || Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                    saveNames = true;
            }

            if (bookNameArea.Contains(MouseCanvas) && SelectedBook is BasicQuestBook book)
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ChangeQuestBookName");

                if (LeftMouseJustReleased)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    typingBookName = !typingBookName;
                    saveNames |= !typingBookName;
                    typingChapterName = false;

                    if (typingBookName)
                        lastBookName = book.NameKey;
                }
            }

            else if (chapterNameArea.Contains(MouseCanvas) && SelectedChapter is BasicChapter chapter)
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ChangeChapterName");

                if (LeftMouseJustReleased)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    typingChapterName = !typingChapterName;
                    saveNames |= !typingChapterName;
                    typingBookName = false;

                    if (typingChapterName)
                        lastChapterName = chapter.NameKey;
                }
            }

            else if (LeftMouseJustPressed && (typingBookName || typingChapterName))
                saveNames = true;

            if (saveNames)
            {
                if (lastBookName is not null && SelectedBook is BasicQuestBook basicQuestBook && basicQuestBook.NameKey != lastBookName)
                {
                    string lastNameKey = lastBookName;
                    string newNameKey = basicQuestBook.NameKey;

                    AddHistory(() => {
                        basicQuestBook.NameKey = lastNameKey;
                    }, () => {
                        basicQuestBook.NameKey = newNameKey;
                    });

                    lastBookName = newNameKey;
                }

                else if (lastChapterName is not null && SelectedChapter is BasicChapter basic && basic.NameKey != lastChapterName)
                {
                    string lastNameKey = lastChapterName;
                    string newNameKey = basic.NameKey;

                    AddHistory(() => {
                        basic.NameKey = lastNameKey;
                    }, () => {
                        basic.NameKey = newNameKey;
                    });

                    lastChapterName = newNameKey;
                }

                typingBookName = false;
                typingChapterName = false;
            }

            var font = FontAssets.DeathText.Value;

            if (SelectedBook is BasicQuestBook basicBook)
            {
                AddRectangle(bookNameArea, Color.Gray * 0.6f, fill: true);
                DrawTasks.Add(sb => sb.DrawOutlinedStringInRectangle(bookNameArea.CookieCutter(new(0f, -1.5f), Vector2.One), font, Color.White, Color.Black, Language.GetTextValue("Mods.QuestBooks.Tooltips.LocalizationKey"), maxScale: 0.5f));

                if (typingBookName)
                {
                    AddRectangle(bookNameArea, Color.Lerp(Color.Black, Color.Yellow, colorLerp), 3f);
                    DrawTasks.Add(_ =>
                    {
                        PlayerInput.WritingText = true;
                        Main.instance.HandleIME();
                    });

                    string newNameKey = Main.GetInputText(basicBook.NameKey);
                    if (basicBook.NameKey != newNameKey)
                    {
                        basicBook.NameKey = newNameKey;
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                }

                else
                    AddRectangle(bookNameArea, bookNameArea.Contains(MouseCanvas) ? Color.LightGray : Color.Black, 3f);

                DrawTasks.Add(sb => sb.DrawOutlinedStringInRectangle(bookNameArea.CookieCutter(new(0f, 0.15f), Vector2.One).CreateMargins(left: 2, right: 3), FontAssets.DeathText.Value, Color.White, Color.Black, basicBook.NameKey, minimumScale: 0.4f, alignment: Utilities.TextAlignment.Right));

            }

            if (SelectedChapter is BasicChapter basicChapter && (SelectedBook?.Chapters.Contains(SelectedChapter) ?? false))
            {
                AddRectangle(chapterNameArea, Color.Gray * 0.6f, fill: true);
                DrawTasks.Add(sb => sb.DrawOutlinedStringInRectangle(chapterNameArea.CookieCutter(new(0f, -1.5f), Vector2.One), font, Color.White, Color.Black, Language.GetTextValue("Mods.QuestBooks.Tooltips.LocalizationKey"), maxScale: 0.5f));

                if (typingChapterName)
                {
                    AddRectangle(chapterNameArea, Color.Lerp(Color.Yellow, Color.Black, colorLerp), 3f);
                    DrawTasks.Add(_ =>
                    {
                        PlayerInput.WritingText = true;
                        Main.instance.HandleIME();
                    });

                    string newChapterKey = Main.GetInputText(basicChapter.NameKey);
                    if (basicChapter.NameKey != newChapterKey)
                    {
                        basicChapter.NameKey = newChapterKey;
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                }

                else
                    AddRectangle(chapterNameArea, chapterNameArea.Contains(MouseCanvas) ? Color.LightGray : Color.Black, 3f);

                DrawTasks.Add(sb => sb.DrawOutlinedStringInRectangle(chapterNameArea.CookieCutter(new(0f, 0.15f), Vector2.One).CreateMargins(left: 2, right: 3), FontAssets.DeathText.Value, Color.White, Color.Black, basicChapter.NameKey, minimumScale: 0.4f, alignment: Utilities.TextAlignment.Right));
            }
        }
    }
}

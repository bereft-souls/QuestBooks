using Microsoft.Xna.Framework;
using QuestBooks.QuestLog.DefaultChapters;
using QuestBooks.QuestLog.DefaultQuestBooks;
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
        private static bool typingBookName = false;
        private static bool typingChapterName = false;

        private static void HandleRenaming(Rectangle books, Rectangle chapters, Rectangle questArea)
        {
            Rectangle bookNameArea = books.CookieCutter(new(-0.15f, -1.25f), new(1.15f, 0.08f));
            Rectangle chapterNameArea = chapters.CookieCutter(new(0.15f, -1.25f), new(1.15f, 0.08f));

            float colorLerp = (float)(Main.timeForVisualEffects % 60);

            if (colorLerp > 30)
                colorLerp -= (colorLerp % 30) * 2;

            colorLerp /= 30f;

            if (typingBookName || typingChapterName)
            {
                CancelChat = true;

                if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) || Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                {
                    typingBookName = false;
                    typingChapterName = false;
                }
            }
            
            if (bookNameArea.Contains(MouseCanvas) && SelectedBook is not null)
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ChangeQuestBookName");

                if (LeftMouseJustReleased)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    typingBookName = !typingBookName;
                    typingChapterName = false;
                }
            }

            else if (chapterNameArea.Contains(MouseCanvas) && SelectedChapter is not null)
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ChangeChapterName");

                if (LeftMouseJustReleased)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    typingChapterName = !typingChapterName;
                    typingBookName = false;
                }
            }

            else if (LeftMouseJustPressed)
            {
                typingBookName = false;
                typingChapterName = false;
            }

            if (SelectedBook is BasicQuestBook basicBook)
            {
                AddRectangle(bookNameArea, Color.Gray * 0.6f, fill: true);
                var font = FontAssets.DeathText.Value;
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

                if (SelectedChapter is BasicChapter basicChapter && SelectedBook.Chapters.Contains(SelectedChapter))
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
}

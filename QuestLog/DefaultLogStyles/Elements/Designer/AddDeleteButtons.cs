using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultChapters;
using QuestBooks.QuestLog.DefaultQuestBooks;
using QuestBooks.Systems;
using SDL2;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private static void HandleAddDeleteButtons(Rectangle books, Rectangle chapters, Rectangle questArea)
        {
            if (SelectedElement is not null)
                return;

            // Rectangles for adding and deleting
            Rectangle addBook = books.CookieCutter(new(0, -1.05f), new(0.25f, 0.05f));
            Rectangle addChapter = chapters.CookieCutter(new(0, -1.05f), new(0.25f, 0.05f));
            Rectangle deleteBook = books.CookieCutter(new(0.85f, 1.05f), new(0.15f, 0.035f));
            Rectangle deleteChapter = chapters.CookieCutter(new(-0.84f, 1.05f), new(0.15f, 0.035f));

            bool addBookHovered = false;
            bool addChapterHovered = false;
            bool deleteBookHovered = false;
            bool deleteChapterHovered = false;

            if (addBook.Contains(MouseCanvas))
            {
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.AddBook");
                addBookHovered = true;

                // Add new questbook with placeholder localization key
                if (LeftMouseJustReleased)
                {
                    TabBook newBook = new() { NameKey = $"Mods.{QuestBooksMod.DesignerMod.Name}.Book{QuestManager.QuestBooks.Count + 1}" };
                    QuestManager.QuestBooks.Add(newBook);
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }
            }

            else if (addChapter.Contains(MouseCanvas))
            {
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.AddChapter");
                addChapterHovered = true;

                // Add new chapter with placeholder localization key
                if (LeftMouseJustReleased && SelectedBook is not null)
                {
                    ScrollChapter newLine = new() { NameKey = $"Mods.{QuestBooksMod.DesignerMod.Name}.Chapter{SelectedBook.Chapters.Count + 1}" };
                    SelectedBook.Chapters.Add(newLine);
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }
            }

            else if (deleteBook.Contains(MouseCanvas))
            {
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.DeleteBook");
                deleteBookHovered = true;

                if (LeftMouseJustReleased && SelectedBook is not null)
                {
                    // Display a pop up to make sure the user wants to delete the book
                    SDL.SDL_MessageBoxData message = new()
                    {
                        window = Main.instance.Window.Handle,
                        title = "Delete Quest Book",
                        message = $"Are you sure you want to delete the selected quest book: {SelectedBook.DisplayName}?\n" +
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
                        QuestManager.QuestBooks.Remove(SelectedBook);
                        DrawTasks.Add(_ => SelectedBook = null);
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }

                    else
                        SoundEngine.PlaySound(SoundID.MenuClose);
                }
            }

            else if (deleteChapter.Contains(MouseCanvas))
            {
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.DeleteChapter");
                deleteChapterHovered = true;

                if (LeftMouseJustReleased && (SelectedBook?.Chapters.Contains(SelectedChapter) ?? false))
                {
                    // Display a pop up to make sure the user wants to delete the chapter
                    SDL.SDL_MessageBoxData message = new()
                    {
                        window = Main.instance.Window.Handle,
                        title = "Delete Chapter",
                        message = $"Are you sure you want to delete the selected chapter: {SelectedChapter.DisplayName}?\n" +
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
                        SelectedBook.Chapters.Remove(SelectedChapter);
                        DrawTasks.Add(_ =>
                        {
                            SelectedChapter = null;
                            SortedElements = null;
                        });
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }

                    else
                        SoundEngine.PlaySound(SoundID.MenuClose);
                }
            }

            DrawTasks.Add(sb =>
            {
                Texture2D addButton = QuestAssets.AddButton;
                Texture2D addHovered = QuestAssets.AddButtonHovered;
                Texture2D deleteButton = QuestAssets.DeleteButton;
                Texture2D deleteHovered = QuestAssets.DeleteButtonHovered;

                float addScale = addBook.Width / (float)addButton.Width;
                float deleteScale = deleteBook.Width / (float)deleteButton.Width;

                if (addBookHovered)
                    sb.Draw(addHovered, addBook.Center(), null, Color.White, 0f, addHovered.Size() * 0.5f, addScale, SpriteEffects.None, 0f);
                else
                    sb.Draw(addButton, addBook.Center(), null, Color.White, 0f, addButton.Size() * 0.5f, addScale, SpriteEffects.None, 0f);

                if (SelectedBook is not null)
                {
                    if (deleteBookHovered)
                        sb.Draw(deleteHovered, deleteBook.Center(), null, Color.White, 0f, deleteHovered.Size() * 0.5f, deleteScale, SpriteEffects.None, 0f);
                    else
                        sb.Draw(deleteButton, deleteBook.Center(), null, Color.White, 0f, deleteButton.Size() * 0.5f, deleteScale, SpriteEffects.None, 0f);

                    if (addChapterHovered)
                        sb.Draw(addHovered, addChapter.Center(), null, Color.White, 0f, addHovered.Size() * 0.5f, addScale, SpriteEffects.None, 0f);
                    else
                        sb.Draw(addButton, addChapter.Center(), null, Color.White, 0f, addButton.Size() * 0.5f, addScale, SpriteEffects.None, 0f);

                    if (SelectedBook.Chapters.Contains(SelectedChapter))
                    {
                        if (deleteChapterHovered)
                            sb.Draw(deleteHovered, deleteChapter.Center(), null, Color.White, 0f, deleteHovered.Size() * 0.5f, deleteScale, SpriteEffects.None, 0f);
                        else
                            sb.Draw(deleteButton, deleteChapter.Center(), null, Color.White, 0f, deleteButton.Size() * 0.5f, deleteScale, SpriteEffects.None, 0f);
                    }

                    else
                        sb.Draw(deleteButton, deleteChapter.Center(), null, Color.White * 0.5f, 0f, deleteButton.Size() * 0.5f, deleteScale, SpriteEffects.None, 0f);
                }

                else
                {
                    sb.Draw(addButton, addChapter.Center(), null, Color.White * 0.5f, 0f, addButton.Size() * 0.5f, addScale, SpriteEffects.None, 0f);
                    sb.Draw(deleteButton, deleteBook.Center(), null, Color.White * 0.5f, 0f, deleteButton.Size() * 0.5f, deleteScale, SpriteEffects.None, 0f);
                    sb.Draw(deleteButton, deleteChapter.Center(), null, Color.White * 0.5f, 0f, deleteButton.Size() * 0.5f, deleteScale, SpriteEffects.None, 0f);
                }
            });
        }
    }
}

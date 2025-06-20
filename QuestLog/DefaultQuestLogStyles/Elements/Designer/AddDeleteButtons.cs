using QuestBooks.QuestLog.DefaultQuestBooks;
using QuestBooks.QuestLog.DefaultQuestLines;
using QuestBooks.Systems;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;

namespace QuestBooks.QuestLog.DefaultQuestLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private static void HandleAddDeleteButtons(Rectangle books, Rectangle chapters, Rectangle questArea)
        {
            Rectangle addBook = books.CookieCutter(new(0, -1.05f), new(0.25f, 0.05f));
            Rectangle addChapter = chapters.CookieCutter(new(0, -1.05f), new(0.25f, 0.05f));

            Rectangle deleteBook = books.CookieCutter(new(0.85f, 1.05f), new(0.15f, 0.035f));
            Rectangle deleteChapter = chapters.CookieCutter(new(-0.85f, 1.05f), new(0.15f, 0.035f));

            if (addBook.Contains(MouseCanvas))
            {
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.AddBook");

                if (LeftMouseJustReleased)
                {
                    BasicQuestBook newBook = new() { NameKey = $"YourMod.QuestBooks.Book{QuestManager.QuestBooks.Count + 1}" };
                    QuestManager.QuestBooks.Add(newBook);
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }
            }

            else if (addChapter.Contains(MouseCanvas))
            {
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.AddChapter");

                if (LeftMouseJustReleased && SelectedBook is not null)
                {
                    BasicQuestLine newLine = new() { NameKey = $"YourMod.QuestBooks.Chapter{SelectedBook.QuestLines.Count + 1}" };
                    SelectedBook.QuestLines.Add(newLine);
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }
            }

            else if (deleteBook.Contains(MouseCanvas))
            {
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.DeleteBook");

                if (LeftMouseJustReleased && SelectedBook is not null)
                {
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

                if (LeftMouseJustReleased && (SelectedBook?.QuestLines.Contains(SelectedChapter) ?? false))
                {
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

                    if (result == 0 && buttonId == 1)
                    {
                        SelectedBook.QuestLines.Remove(SelectedChapter);
                        DrawTasks.Add(_ => SelectedChapter = null);
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }

                    else
                        SoundEngine.PlaySound(SoundID.MenuClose);
                }
            }

            AddRectangle(addBook, Color.Orange, fill: true);

            if (SelectedBook is not null)
            {
                AddRectangle(addChapter, Color.Orange, fill: true);
                AddRectangle(deleteBook, Color.Blue, fill: true);

                if (SelectedBook?.QuestLines.Contains(SelectedChapter) ?? false)
                    AddRectangle(deleteChapter, Color.Blue, fill: true);

                else
                    AddRectangle(deleteChapter, Color.Blue * 0.5f, fill: true);
            }

            else
            {
                AddRectangle(addChapter, Color.Orange * 0.5f, fill: true);
                AddRectangle(deleteBook, Color.Blue * 0.5f, fill: true);
                AddRectangle(deleteChapter, Color.Blue * 0.5f, fill: true);
            }
        }
    }
}

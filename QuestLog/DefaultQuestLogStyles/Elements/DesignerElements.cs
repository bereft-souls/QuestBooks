using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultQuestBooks;
using QuestBooks.QuestLog.DefaultQuestLines;
using QuestBooks.Systems;
using SDL2;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.Utilities.FileBrowser;

namespace QuestBooks.QuestLog.DefaultQuestLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private static bool TypingBookName = false;
        private static bool TypingChapterName = false;

        private void UpdateDesigner(Rectangle books, Rectangle chapters, Rectangle questArea)
        {
            //AddRectangle(books, Color.Yellow);
            //AddRectangle(chapters, Color.Yellow);
            //AddRectangle(questArea, Color.Yellow);

            DrawTasks.Add(sb =>
            {
                sb.End();
                sb.Begin(SpriteSortMode.Deferred, CustomBlendState, SamplerState.LinearClamp, CustomDepthStencilState, CustomRasterizerState);
            });

            #region Book/Chapter Renaming

            Rectangle bookNameArea = books.CookieCutter(new(-0.15f, -1.25f), new(1.15f, 0.08f));
            Rectangle chapterNameArea = chapters.CookieCutter(new(0.15f, -1.25f), new(1.15f, 0.08f));

            float colorLerp = (float)(Main.timeForVisualEffects % 60);

            if (colorLerp > 30)
                colorLerp -= (colorLerp % 30) * 2;

            colorLerp /= 30f;

            if ((Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) || Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                && (TypingBookName || TypingChapterName))
            {
                TypingBookName = false;
                TypingChapterName = false;
                Main.drawingPlayerChat = false;
            }

            else if (bookNameArea.Contains(MouseCanvas) && SelectedBook is not null)
            {
                LockMouse();

                if (LeftMouseJustReleased)
                {
                    TypingBookName = true;
                    TypingChapterName = false;
                }
            }

            else if (chapterNameArea.Contains(MouseCanvas) && SelectedChapter is not null)
            {
                LockMouse();

                if (LeftMouseJustReleased)
                {
                    TypingChapterName = true;
                    TypingBookName = false;
                }
            }

            else if (LeftMouseJustReleased)
            {
                TypingBookName = false;
                TypingChapterName = false;
            }

            if (SelectedBook is not null)
            {
                AddRectangle(bookNameArea, Color.Gray * 0.6f, fill: true);

                if (TypingBookName)
                {
                    AddRectangle(bookNameArea, Color.Lerp(Color.Black, Color.Yellow, colorLerp), 3f);
                    DrawTasks.Add(_ =>
                    {
                        PlayerInput.WritingText = true;
                        Main.instance.HandleIME();
                    });

                    string newNameKey = Main.GetInputText(SelectedBook.NameKey);
                    SelectedBook.NameKey = newNameKey;
                }

                else
                    AddRectangle(bookNameArea, Color.Black, 3f);

                DrawTasks.Add(sb => sb.DrawStringInRectangle(bookNameArea.CreateScaledMargin(0.01f).CreateScaledMargins(top: 0.125f), FontAssets.DeathText.Value, Color.White, SelectedBook.NameKey, 0.4f, alignment: Utilities.TextAlignment.Right));

                if (SelectedChapter is not null && SelectedBook.QuestLines.Contains(SelectedChapter))
                {
                    AddRectangle(chapterNameArea, Color.Gray * 0.6f, fill: true);

                    if (TypingChapterName)
                    {
                        AddRectangle(chapterNameArea, Color.Lerp(Color.Yellow, Color.Black, colorLerp), 3f);
                        DrawTasks.Add(_ =>
                        {
                            PlayerInput.WritingText = true;
                            Main.instance.HandleIME();
                        });

                        string newChapterKey = Main.GetInputText(SelectedChapter.NameKey);
                        SelectedChapter.NameKey = newChapterKey;
                    }

                    else
                        AddRectangle(chapterNameArea, Color.Black, 3f);

                    DrawTasks.Add(sb => sb.DrawStringInRectangle(chapterNameArea.CreateScaledMargin(0.01f).CreateScaledMargins(top: 0.125f), FontAssets.DeathText.Value, Color.White, SelectedChapter.NameKey, 0.4f, alignment: Utilities.TextAlignment.Right));
                }
            }

            #endregion

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
                }
            }

            else if (addChapter.Contains(MouseCanvas))
            {
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.AddChapter");

                if (LeftMouseJustReleased && SelectedBook is not null)
                {
                    BasicQuestLine newLine = new() { NameKey = $"YourMod.QuestBooks.Chapter{SelectedBook.QuestLines.Count + 1}" };
                    SelectedBook.QuestLines.Add(newLine);
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

                    int result = SDL.SDL_ShowMessageBox(ref message, out int buttonId);

                    if (result == 0 && buttonId == 1)
                    {
                        QuestManager.QuestBooks.Remove(SelectedBook);
                        DrawTasks.Add(_ => SelectedBook = null);
                    }
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

                    int result = SDL.SDL_ShowMessageBox(ref message, out int buttonId);

                    if (result == 0 && buttonId == 1)
                    {
                        SelectedBook.QuestLines.Remove(SelectedChapter);
                        DrawTasks.Add(_ => SelectedChapter = null);
                    }
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

            Rectangle saveAll = LogArea.CookieCutter(new(-1.06f, -0.94f), new(0.05f, -0.05f));
            Rectangle saveSelected = saveAll.CookieCutter(new(0f, 2.5f), Vector2.One);
            Rectangle loadAll = saveSelected.CookieCutter(new(0f, 2.5f), Vector2.One);
            Rectangle loadSingle = loadAll.CookieCutter(new(0f, 2.5f), Vector2.One);

            AddRectangle(saveAll, Color.Magenta, fill: true);
            AddRectangle(saveSelected, Color.Purple, fill: true);
            AddRectangle(loadAll, Color.LightSeaGreen, fill: true);
            AddRectangle(loadSingle, Color.DarkGreen, fill: true);

            if (saveAll.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.SaveAll");

                if (LeftMouseJustReleased)
                {
                    var folder = NativeFileDialogSharp.Dialog.FolderPicker();
                    if (folder.IsOk)
                    {
                        foreach (var book in AvailableBooks)
                        {
                            string filePath = folder.Path + Path.DirectorySeparatorChar + book.DisplayName + ".json";
                            QuestLoader.SaveQuestBook(book, filePath);
                        }

                        Main.NewText("Quest books exported!");
                    }
                }
            }

            else if (saveSelected.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.SaveSelected");

                if (LeftMouseJustReleased && SelectedBook is not null)
                {
                    var file = NativeFileDialogSharp.Dialog.FileSave("json", null);

                    if (file.IsOk)
                    {
                        QuestLoader.SaveQuestBook(SelectedBook, file.Path);
                        Main.NewText("Quest book saved!");
                    }
                }
            }

            else if (loadAll.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.LoadAll");

                if (LeftMouseJustReleased)
                {
                    SDL.SDL_MessageBoxData message = new()
                    {
                        window = Main.instance.Window.Handle,
                        title = "Load Multiple",
                        message = "Are you sure you want to load multiple QuestBooks?\n" +
                        "This will clear any quest books currently in the editor, so make sure to save your progress!",
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
                                text = "Continue"
                            }
                        ]
                    };

                    int result = SDL.SDL_ShowMessageBox(ref message, out int buttonId);

                    if (result == 0 && buttonId == 1)
                    {
                        var files = NativeFileDialogSharp.Dialog.FileOpenMultiple("json", null);
                        if (files.IsOk)
                        {
                            bool cleared = false;

                            foreach (var file in files.Paths)
                            {
                                QuestBook book = QuestLoader.LoadQuestBook(file);

                                if (book is not BasicQuestBook questBook)
                                {
                                    Main.NewText($"Unable to parse file into BasicQuestBook: {file}");
                                    continue;
                                }

                                if (!cleared)
                                {
                                    DrawTasks.Add(_ =>
                                    {
                                        SelectedBook = null;
                                        SelectedChapter = null;
                                        QuestManager.QuestBooks.Clear();
                                    });

                                    cleared = true;
                                }

                                DrawTasks.Add(_ => QuestManager.QuestBooks.Add(questBook));
                            }

                            if (cleared)
                                Main.NewText("Quest books loaded!");
                        }
                    }
                }   
            }

            else if (loadSingle.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.LoadBook");

                if (LeftMouseJustReleased)
                {
                    var file = NativeFileDialogSharp.Dialog.FileOpen("json", null);
                    if (file.IsOk)
                    {
                        QuestBook book = QuestLoader.LoadQuestBook(file.Path);

                        if (book is not BasicQuestBook questBook)
                            Main.NewText($"Unable to parse file into BasicQuestBook: {file}");

                        else
                        {
                            QuestManager.QuestBooks.Add(book);
                            Main.NewText("Quest book loaded!");
                        }
                    }
                }
            }

            DrawTasks.Add(sb =>
            {
                sb.End();
                sb.Begin(SpriteSortMode.Deferred, CustomBlendState, CustomSamplerState, CustomDepthStencilState, CustomRasterizerState);
            });
        }
    }
}

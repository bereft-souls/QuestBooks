using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Systems;
using SDL2;
using System.IO;
using Terraria;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private static void HandleSaveLoadButtons()
        {
            Rectangle saveAll = LogArea.CookieCutter(new(-1.06f, -0.94f), new(0.05f, 0.05f));
            Rectangle saveSelected = saveAll.CookieCutter(new(0f, 2.5f), Vector2.One);
            Rectangle loadAll = saveSelected.CookieCutter(new(0f, 2.5f), Vector2.One);
            Rectangle loadSingle = loadAll.CookieCutter(new(0f, 2.5f), Vector2.One);

            bool saveAllHovered = false;
            bool saveSelectedHovered = false;
            bool loadAllHovered = false;
            bool loadSingleHovered = false;

            if (saveAll.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.SaveAll");
                saveAllHovered = true;

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

                        Main.NewText(Language.GetTextValue("Mods.QuestBooks.ChatMessages.MultipleBooksExported"));
                    }
                }
            }

            else if (saveSelected.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.SaveSelected");
                saveSelectedHovered = true;

                if (LeftMouseJustReleased && SelectedBook is not null)
                {
                    var file = NativeFileDialogSharp.Dialog.FileSave("json", null);

                    if (file.IsOk)
                    {
                        QuestLoader.SaveQuestBook(SelectedBook, file.Path);
                        Main.NewText(Language.GetTextValue("Mods.QuestBooks.ChatMessages.SingleBookExported"));
                    }
                }
            }

            else if (loadAll.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.LoadAll");
                loadAllHovered = true;

                if (LeftMouseJustReleased)
                {
                    // Show the user a message to ensure they want to overwrite data
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

                    // If okay...
                    if (result == 0 && buttonId == 1)
                    {
                        var files = NativeFileDialogSharp.Dialog.FileOpenMultiple("json", null);
                        if (files.IsOk)
                        {
                            bool cleared = false;

                            foreach (var file in files.Paths)
                            {
                                var book = QuestLoader.LoadQuestBook(file);
                                if (book is not QuestBook questBook)
                                {
                                    Main.NewText(Language.GetTextValueWith("Mods.QuestBooks.ChatMessages.ParseError", file));
                                    continue;
                                }

                                if (!cleared)
                                {
                                    DrawTasks.Add(_ =>
                                    {
                                        SelectedBook = null;
                                        SelectedChapter = null;
                                        SelectedElement = null;

                                        QuestAreaOffset = Vector2.Zero;
                                        questElementSwipeOffset = questAreaTarget.Width;
                                        SortedElements = null;

                                        QuestManager.QuestBooks.Clear();
                                    });

                                    cleared = true;
                                }

                                DrawTasks.Add(_ => QuestManager.QuestBooks.Add(questBook));
                            }

                            if (cleared)
                                Main.NewText(Language.GetTextValue("Mods.QuestBooks.ChatMessages.MultipleBooksImported"));
                        }
                    }
                }
            }

            else if (loadSingle.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.LoadBook");
                loadSingleHovered = true;

                if (LeftMouseJustReleased)
                {
                    var file = NativeFileDialogSharp.Dialog.FileOpen("json", null);
                    if (file.IsOk)
                    {
                        try
                        {
                            var book = QuestLoader.LoadQuestBook(file.Path);
                            QuestManager.QuestBooks.Add(book);
                            Main.NewText(Language.GetTextValue("Mods.QuestBooks.ChatMessages.SingleBookImported"));
                        }
                        catch
                        {
                            Main.NewText(Language.GetTextValueWith("Mods.QuestBooks.ChatMessages.ParseError", file));
                        }
                    }
                }
            }

            DrawTasks.Add(sb =>
            {
                Texture2D texture = saveAllHovered ? QuestAssets.ExportAllButtonHovered : QuestAssets.ExportAllButton;
                float scale = saveAll.Width / (float)QuestAssets.ExportAllButton.Asset.Width;
                sb.Draw(texture, saveAll.Center(), null, Color.White, 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);

                texture = saveSelectedHovered ? QuestAssets.ExportButtonHovered : QuestAssets.ExportButton;
                sb.Draw(texture, saveSelected.Center(), null, Color.White, 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);

                texture = loadAllHovered ? QuestAssets.ImportAllButtonHovered : QuestAssets.ImportAllButton;
                sb.Draw(texture, loadAll.Center(), null, Color.White, 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);

                texture = loadSingleHovered ? QuestAssets.ImportButtonHovered : QuestAssets.ImportButton;
                sb.Draw(texture, loadSingle.Center(), null, Color.White, 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            });
        }
    }
}

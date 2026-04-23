using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Systems;
using SDL2;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultStyles
{
    public partial class BasicQuestLogStyle
    {
        private void HandleSaveLoadButtons()
        {
            Rectangle saveBook = LogArea.CookieCutter(new(-1.06f, -0.94f), new(0.05f, 0.05f));
            Rectangle loadBook = saveBook.CookieCutter(new(0f, 2.5f), Vector2.One);

            bool saveBookHovered = false;
            bool loadBookHovered = false;

            if (saveBook.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.SaveLog");
                saveBookHovered = true;

                if (LeftMouseJustReleased)
                {
                    var file = NativeFileDialogSharp.Dialog.FileSave("json", null);

                    if (file.IsOk)
                    {
                        string path = file.Path;

                        if (Path.GetExtension(path) != ".json")
                        {
                            path += ".json";
                            if (File.Exists(path))
                            {
                                SDL.SDL_MessageBoxData message = new()
                                {
                                    window = Main.instance.Window.Handle,
                                    title = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.FileExistsTitle"),
                                    message = Language.GetText("Mods.QuestBooks.Tooltips.Designer.FileExistsMessage").Format(path),
                                    flags = SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_WARNING,
                                    numbuttons = 2,
                                    buttons = [
                                        new SDL.SDL_MessageBoxButtonData()
                                        {
                                            buttonid = 2,
                                            flags = SDL.SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_ESCAPEKEY_DEFAULT,
                                            text = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.No"),
                                        },
                                        new SDL.SDL_MessageBoxButtonData()
                                        {
                                            buttonid = 1,
                                            flags = SDL.SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_RETURNKEY_DEFAULT,
                                            text = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.Yes"),
                                        }
                                    ]
                                };

                                int result = SDL.SDL_ShowMessageBox(ref message, out int buttonid);
                                if (result != 0 || buttonid != 1)
                                    path = null;
                            }
                        }

                        if (path is not null)
                        {
                            QuestLoader.SaveQuestLog(QuestManager.QuestBooks, path);
                            Main.NewText(Language.GetTextValue("Mods.QuestBooks.ChatMessages.Designer.QuestLogExported"));
                        }
                    }
                }
            }

            else if (loadBook.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.LoadLog");
                loadBookHovered = true;

                if (LeftMouseJustReleased)
                {
                    // Show the user a message to ensure they want to overwrite data
                    SDL.SDL_MessageBoxData message = new()
                    {
                        window = Main.instance.Window.Handle,
                        title = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ConfirmLoadLogTitle"),
                        message = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ConfirmLoadLogMessage"),
                        flags = SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_WARNING,
                        numbuttons = 2,
                        buttons = [
                            new SDL.SDL_MessageBoxButtonData()
                            {
                                buttonid = 2,
                                flags = SDL.SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_ESCAPEKEY_DEFAULT,
                                text = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.No")
                            },
                            new SDL.SDL_MessageBoxButtonData()
                            {
                                buttonid = 1,
                                flags = SDL.SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_RETURNKEY_DEFAULT,
                                text = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.Yes")
                            }
                        ]
                    };

                    int result = SDL.SDL_ShowMessageBox(ref message, out int buttonId);

                    // If okay...
                    if (result == 0 && buttonId == 1)
                    {
                        var file = NativeFileDialogSharp.Dialog.FileOpen("json", null);
                        List<QuestBook> questLog = null;

                        if (file.IsOk)
                        {
                            try
                            {
                                questLog = QuestLoader.LoadQuestLog(file.Path);
                                Main.NewText(Language.GetTextValue("Mods.QuestBooks.ChatMessages.Designer.QuestLogImported"));
                            }
                            catch
                            {
                                Main.NewText(Language.GetText("Mods.QuestBooks.ChatMessages.Designer.ParseError").Format(file));
                            }
                        }

                        DrawTasks.Add(_ =>
                        {
                            SelectedBook = null;
                            SelectedChapter = null;
                            SelectedElement = null;

                            QuestAreaOffset = Vector2.Zero;
                            questElementSwipeOffset = questAreaTarget.Width;
                            SortedElements = null;

                            QuestManager.QuestLogs.Remove("Editor");
                            QuestManager.QuestLogs.Add("Editor", questLog);
                            QuestManager.SelectQuestLog("Editor");
                        });
                    }
                }
            }

            DrawTasks.Add(sb =>
            {
                Texture2D texture = saveBookHovered ? QuestAssets.ExportButtonHovered : QuestAssets.ExportButton;
                float scale = saveBook.Width / (float)QuestAssets.ExportButton.Asset.Width;
                sb.Draw(texture, saveBook.Center(), null, Color.White, 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);

                texture = loadBookHovered ? QuestAssets.ImportButtonHovered : QuestAssets.ImportButton;
                sb.Draw(texture, loadBook.Center(), null, Color.White, 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            });
        }
    }
}

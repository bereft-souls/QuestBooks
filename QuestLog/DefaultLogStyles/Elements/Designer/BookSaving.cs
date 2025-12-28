using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Systems;
using SDL2;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultLogStyles
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
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.SaveLog");
                saveBookHovered = true;

                if (LeftMouseJustReleased)
                {
                    var file = NativeFileDialogSharp.Dialog.FileSave("json", null);

                    if (file.IsOk)
                    {
                        QuestLoader.SaveQuestLog(QuestManager.QuestBooks, file.Path);
                        Main.NewText(Language.GetTextValue("Mods.QuestBooks.ChatMessages.QuestLogExported"));
                    }
                }
            }

            else if (loadBook.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.LoadLog");
                loadBookHovered = true;

                if (LeftMouseJustReleased)
                {
                    // Show the user a message to ensure they want to overwrite data
                    SDL.SDL_MessageBoxData message = new()
                    {
                        window = Main.instance.Window.Handle,
                        title = "Load Multiple",
                        message = "Are you sure you want to load a new QuestLog?\n" +
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
                        var file = NativeFileDialogSharp.Dialog.FileOpen("json", null);
                        List<QuestBook> questLog = null;

                        if (file.IsOk)
                        {
                            try
                            {
                                questLog = QuestLoader.LoadQuestLog(file.Path);
                                Main.NewText(Language.GetTextValue("Mods.QuestBooks.ChatMessages.QuestLogImported"));
                            }
                            catch
                            {
                                Main.NewText(Language.GetTextValueWith("Mods.QuestBooks.ChatMessages.ParseError", file));
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

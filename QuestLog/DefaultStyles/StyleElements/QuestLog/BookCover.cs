using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Systems;
using System;
using System.Linq;
using Terraria;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultStyles
{
    public partial class BasicQuestLogStyle
    {
        private bool selectingQuestLog = false;
        private bool newLogSelected = false;

        private float logSelectionOffset = 0f;
        private float logSelectionOpacity = 0f;

        private void HandleBookCover(Vector2 questLogCenter)
        {
            Vector2 coverSize = QuestAssets.ClosedBook.Asset.Size() * LogScale;
            Rectangle coverRectangle = CenteredRectangle(questLogCenter, coverSize);
            Rectangle switchLog = coverRectangle.CookieCutter(new(0f, -1.15f), new(0.8f, 0.12f));

            if (selectingQuestLog)
            {
                HandleLogSelection(questLogCenter, switchLog);
                return;
            }

            bool coverRectangleHovered = false;
            bool switchLogHovered = false;

            if (coverRectangle.Contains(MouseCanvas))
            {
                LockMouse();
                coverRectangleHovered = true;

                if (LeftMouseJustReleased)
                {
                    pageFlippingTimer = 45;
                    onCoverPage = false;
                }
            }

            else if (switchLog.Contains(MouseCanvas))
            {
                LockMouse();
                switchLogHovered = true;
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.SelectQuestLog");

                if (LeftMouseJustReleased)
                {
                    newLogSelected = false;
                    logSelectionOffset = -1;
                    selectingQuestLog = true;
                }
            }

            DrawTasks.Add(sb =>
            {
                if (coverRectangleHovered)
                {
                    Texture2D coverOutlineTexture = QuestAssets.ClosedBookOutline;
                    sb.Draw(coverOutlineTexture, questLogCenter, null, Color.White, 0f, coverOutlineTexture.Size() * 0.5f, LogScale, SpriteEffects.None, 0f);
                }

                Texture2D coverTexture = QuestAssets.ClosedBook;
                sb.Draw(coverTexture, questLogCenter, null, Color.White, 0f, coverTexture.Size() * 0.5f, LogScale, SpriteEffects.None, 0f);

                QuestLogDrawer.CoverDrawCalls[QuestManager.ActiveQuestLog](sb, questLogCenter, 0f, LogScale, 1f);
                string title = QuestLogDrawer.LogTitleRetrievalCalls[QuestManager.ActiveQuestLog](QuestManager.ActiveQuestLog);
                QuestLogDrawer.LogTitleDrawCalls[QuestManager.ActiveQuestLog](sb, switchLog, title, 1f, switchLogHovered, true);
            });
        }

        private void HandleLogSelection(Vector2 questLogCenter, Rectangle switchLogLocation)
        {
            if (newLogSelected)
            {
                if (logSelectionOpacity > 0f)
                {
                    logSelectionOpacity -= 0.025f;
                    logSelectionOffset = float.Lerp(logSelectionOffset, 0f, 0.15f);
                }

                else
                {
                    logSelectionOpacity = 0f;
                    logSelectionOffset = 0f;
                    selectingQuestLog = false;
                }
            }

            else
            {
                if (logSelectionOpacity < 1f)
                {
                    logSelectionOpacity += 0.025f;
                    logSelectionOffset = float.Lerp(logSelectionOffset, 0f, 0.15f);
                }

                else
                {
                    logSelectionOpacity = 1f;
                    logSelectionOffset = 0f;
                }
            }

            DrawTasks.Add(sb =>
            {
                Texture2D coverTexture = QuestAssets.ClosedBook;
                float coverOpacity = (float)Math.Pow(1d - logSelectionOpacity, 2d);
                sb.Draw(coverTexture, questLogCenter, null, Color.White * coverOpacity, 0f, coverTexture.Size() * 0.5f, LogScale, SpriteEffects.None, 0f);
                QuestLogDrawer.CoverDrawCalls[QuestManager.ActiveQuestLog](sb, questLogCenter, 0f, LogScale, coverOpacity);
            });

            Rectangle drawArea = switchLogLocation;

            foreach (string log in QuestManager.AvailableQuestLogs.Select(kvp => kvp.Key))
            {
                float opacity = logSelectionOpacity;
                Rectangle logArea = drawArea;

                if (log == QuestManager.ActiveQuestLog)
                {
                    opacity = 1f;

                    if (newLogSelected)
                    {
                        logArea = switchLogLocation;
                        logArea.Y += (int)logSelectionOffset;
                    }

                    else
                    {
                        if (logSelectionOffset < 0f)
                            logSelectionOffset = logArea.Y - switchLogLocation.Y;

                        logArea.Y -= (int)logSelectionOffset;
                    }
                }

                bool logAreaHovered = (!newLogSelected && logArea.Contains(MouseCanvas)) || (newLogSelected && log == QuestManager.ActiveQuestLog);
                if (logAreaHovered && LeftMouseJustReleased)
                {
                    QuestManager.SelectQuestLog(log);
                    newLogSelected = true;
                    logSelectionOffset = logArea.Y - switchLogLocation.Y;
                }

                DrawTasks.Add(sb =>
                {
                    string title = QuestLogDrawer.LogTitleRetrievalCalls[log](log);
                    QuestLogDrawer.LogTitleDrawCalls[log](sb, logArea, title, opacity, logAreaHovered, true);
                });

                drawArea = drawArea.CookieCutter(new(0f, 2.5f), Vector2.One);
            }
        }

        private void HandleCoverToggle()
        {
            Rectangle coverToggle = LogArea.CookieCutter(new(-1f, 1.03f), new(0.075f, 0.05f));
            bool coverToggleHovered = false;

            if (coverToggle.Contains(MouseCanvas))
            {
                LockMouse();
                coverToggleHovered = true;
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Library.BackToCover");

                if (LeftMouseJustReleased)
                {
                    pageFlippingTimer = 45;
                    onCoverPage = true;
                }
            }

            DrawTasks.Add(sb =>
            {
                Texture2D backToCover = QuestAssets.BackToCover;
                float scale = coverToggle.Width / (float)backToCover.Width;
                sb.Draw(backToCover, coverToggle.Center(), null, coverToggleHovered ? Color.LightCyan : Color.White, 0f, backToCover.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            });
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Systems;
using Terraria;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultStyles
{
    public partial class BasicQuestLogStyle
    {
        private void HandleBookCover(Vector2 questLogCenter)
        {
            Vector2 coverSize = QuestAssets.ClosedBook.Asset.Size() * LogScale;
            Rectangle coverRectangle = CenteredRectangle(questLogCenter, coverSize);
            bool coverRectangleHovered = false;

            Rectangle switchBook = coverRectangle.CookieCutter(new(0f, -1.25f), new(0.8f, 0.12f));
            bool switchBookHovered = false;

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

            else if (switchBook.Contains(MouseCanvas))
            {
                LockMouse();
                switchBookHovered = true;
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.SelectQuestBook");

                if (LeftMouseJustReleased)
                {

                }
            }

            DrawTasks.Add(sb =>
            {
                Vector2 drawPos = coverRectangle.Center();

                if (coverRectangleHovered)
                {
                    Texture2D coverOutlineTexture = QuestAssets.ClosedBookOutline;
                    sb.Draw(coverOutlineTexture, drawPos, null, Color.White, 0f, coverOutlineTexture.Size() * 0.5f, LogScale, SpriteEffects.None, 0f);
                }

                Texture2D coverTexture = QuestAssets.ClosedBook;
                sb.Draw(coverTexture, drawPos, null, Color.White, 0f, coverTexture.Size() * 0.5f, LogScale, SpriteEffects.None, 0f);

                QuestLogDrawer.CoverDrawCalls[QuestManager.ActiveQuestLog](sb, drawPos, LogScale, 0f);
                string title = QuestLogDrawer.LogTitleRetrievalCalls[QuestManager.ActiveQuestLog](QuestManager.ActiveQuestLog);
                QuestLogDrawer.LogTitleDrawCalls[QuestManager.ActiveQuestLog](sb, switchBook, title, 1f, switchBookHovered, true);
            });
        }

        private void HandleCoverToggle()
        {
            Rectangle coverToggle = LogArea.CookieCutter(new(-1f, 1f), new(0.1f, 0.1f));
            bool coverToggleHovered = false;

            if (coverToggle.Contains(MouseCanvas))
            {
                LockMouse();
                coverToggleHovered = true;
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.BackToCover");

                if (LeftMouseJustReleased)
                {
                    pageFlippingTimer = 45;
                    onCoverPage = true;
                }
            }

            AddRectangle(coverToggle, coverToggleHovered ? Color.Yellow : Color.LightBlue, fill: true);
        }
    }
}

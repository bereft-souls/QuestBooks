using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests
{
    public abstract class DynamicQuest : Quest
    {
        /// <summary>
        /// Draws the "completed" quest icon to the canvas.
        /// </summary>
        public abstract void DrawCompleted(SpriteBatch spriteBatch, Vector2 canvasOffset, float zoom, bool hovered, bool selected);

        /// <summary>
        /// Draws the "incompleted" quest icon to the canvas.
        /// </summary>
        public abstract void DrawIncomplete(SpriteBatch spriteBatch, Vector2 canvasOffset, float zoom, bool hovered, bool selected);

        /// <summary>
        /// Draws the "locked" quest icon to the canvas.
        /// </summary>
        public abstract void DrawLocked(SpriteBatch spriteBatch, Vector2 canvasOffset, float zoom, bool hovered, bool selected);

        /// <summary>
        /// Determines how big the "hoverable" area for this element should be.<br/>
        /// Typically just the size of your icon asset (or frame size if the icon is animated).
        /// </summary>
        public abstract Vector2 HoverAreaSize(bool unlocked);
    }
}

using Microsoft.Xna.Framework;

namespace QuestBooks.QuestLog
{
    public abstract class QuestLineElement
    {
        public virtual void Update() { }

        /// <summary>
        /// Determines the layer this element should draw to.<br/>
        /// <c>0f</c> is closer to the background, <c>1f</c> the foreground.
        /// </summary>
        public virtual float DrawPriority { get => 0.5f; }

        public abstract void DrawToCanvas(Vector2 offset);
    }
}

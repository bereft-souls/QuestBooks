using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Quests;
using System.Collections;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace QuestBooks.QuestLog
{
    [ExtendsFromMod("QuestBooks")]
    public abstract class QuestLineElement
    {
        public virtual bool HasInfoPage { get => false; }

        public virtual void Update() { }

        public abstract void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset);

        /// <summary>
        /// Determines the layer this element should draw to.<br/>
        /// <c>0f</c> is closer to the background, <c>1f</c> the foreground.
        /// </summary>
        public virtual float DrawPriority { get => 0.5f; }

        public class PrioritySort : IComparer<QuestLineElement>
        {
            public int Compare(QuestLineElement x, QuestLineElement y)
            {
                return x.DrawPriority.CompareTo(y.DrawPriority);
            }
        }
    }
}

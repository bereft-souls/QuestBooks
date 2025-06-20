using System.Collections.Generic;

namespace QuestBooks.QuestLog
{
    public abstract class QuestBook
    {
        public abstract IEnumerable<QuestLine> Chapters { get; }

        public virtual void Update() { }

        /// <summary>
        /// Override this to change how the "open quest log" icon is drawn in the inventory.<br/>
        /// <paramref name="drawPriority"/> is the current draw override priority. If you have something of higher priority, modify both <paramref name="drawPriority"/> and <paramref name="iconDraw"/>.
        /// </summary>
        public virtual void OverrideIconDraw(ref float drawPriority, ref QuestLogStyle.IconDrawDelegate iconDraw)
        {
            foreach (var questLine in Chapters)
                questLine.OverrideIconDraw(ref drawPriority, ref iconDraw);
        }

        /// <summary>
        /// Override this to change how the "open quest log" outline is drawn in the inventory.<br/>
        /// <paramref name="drawPriority"/> is the current draw override priority. If you have something of higher priority, modify both <paramref name="drawPriority"/> and <paramref name="outlineDraw"/>.
        /// </summary>
        public virtual void OverrideIconOutlineDraw(ref float drawPriority, ref QuestLogStyle.IconDrawDelegate outlineDraw)
        {
            foreach (var questLine in Chapters)
                questLine.OverrideIconOutlineDraw(ref drawPriority, ref outlineDraw);
        }
    }
}

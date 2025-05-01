using QuestBooks.QuestLog.DefaultQuestLineElements.BaseElements;
using QuestBooks.Quests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestBooks.QuestLog.DefaultQuestLines
{
    /// <summary>
    /// Represents a basic <see cref="QuestLine"/> implementation, with a chapter name and contained elements.
    /// </summary>
    public class BasicQuestLine : QuestLine
    {
        /// <summary>
        /// The list of all elements contained within this quest line.
        /// </summary>
        public List<QuestLineElement> Elements = [];

        /// <summary>
        /// A collection of all quests contained by elements in this quest line.<br/>
        /// These quests are all the actual implemented instances, and not duplicates or templates - you can access accurate info or methods from them.
        /// </summary>
        public IEnumerable<Quest> QuestList => Elements.Where(e => e is QuestElement).Cast<QuestElement>().Select(e => e.Quest);

        /// <summary>
        /// The completion progress of this quest line.<br/>
        /// 0f is no quests completed, and 1f is all quests completed.<br/>
        /// Pulls from <see cref="QuestList"/>.
        /// </summary>
        public float Progress => (float)QuestList.Count(q => q.Completed) / Math.Max(1, QuestList.Count());

        /// <summary>
        /// Whether or not all quests in this questline are complete.<br/>
        /// Pulls from <see cref="QuestList"/>
        /// </summary>
        public bool Complete => QuestList.All(q => q.Completed);

        /// <summary>
        /// Forwards <see cref="QuestLineElement.Update"/> calls to each element in the quest line.
        /// </summary>
        public override void Update()
        {
            foreach (QuestLineElement questElement in Elements)
                questElement.Update();
        }

        /// <summary>
        /// Draws this quest line to the chapter selection of the quest book.
        /// </summary>
        public override void DrawChapter()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Draws all elements within this questline to the canvas.
        /// </summary>
        public override void DrawContents()
        {
            throw new NotImplementedException();
        }
    }
}

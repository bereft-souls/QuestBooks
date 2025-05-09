using System.Collections.Generic;

namespace QuestBooks.QuestLog
{
    public abstract class QuestBook
    {
        public abstract IEnumerable<QuestLine> Chapters { get; }

        public virtual void Update() { }
    }
}

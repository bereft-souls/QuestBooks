using System.Collections.Generic;

namespace QuestBooks.QuestLog
{
    public abstract class QuestLine
    {
        public abstract IEnumerable<QuestLineElement> Elements { get; }

        public virtual void Update() { }
    }
}

using QuestBooks.Quests;

namespace QuestBooks.QuestLog.DefaultQuestLineElements.BaseElements
{
    public abstract class QuestElement : QuestLineElement
    {
        public abstract Quest Quest { get; }
    }
}

namespace QuestBooks.Quests
{
    public abstract class Quest
    {
        public bool Completed { get; internal set; }

        public abstract string Key { get; }
        public virtual QuestType QuestType { get => QuestType.World; }

        public virtual void OnCompletion() { }
        public virtual void MarkAsComplete() { }

        public abstract bool CheckCompletion();
    }

    public enum QuestType
    {
        World,
        Player
    }
}

namespace QuestBooks.QuestLog
{
    public abstract class QuestBook
    {
        public virtual void Update() { }

        public abstract void DrawBook();

        public abstract void DrawChapters();
    }
}

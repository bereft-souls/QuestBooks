namespace QuestBooks.QuestLog
{
    public abstract class QuestLine
    {
        public virtual void Update() { }

        public abstract void DrawChapter();

        public abstract void DrawContents();
    }
}

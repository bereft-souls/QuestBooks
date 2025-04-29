namespace QuestBooks.QuestLog
{
    public abstract class QuestLineElement
    {
        public virtual void Update() { }

        public abstract void DrawToCanvas();
    }
}

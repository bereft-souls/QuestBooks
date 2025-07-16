namespace QuestBooks.QuestLog.DefaultElements
{
    [ElementTooltip("UnderlayElement")]
    public class UnderlayElement : OverlayElement
    {
        public override float DrawPriority => 0.1f;
    }
}

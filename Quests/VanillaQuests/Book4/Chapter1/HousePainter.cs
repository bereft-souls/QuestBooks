namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HousePainter : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Painter);
}
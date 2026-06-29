namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseStylist : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Stylist);
}
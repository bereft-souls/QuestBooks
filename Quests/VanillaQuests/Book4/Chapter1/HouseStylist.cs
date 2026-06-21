namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseStylist : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Stylist);
}
namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseDyeTrader : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.DyeTrader);
}
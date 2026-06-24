namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseTaxCollector : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.TaxCollector);
}
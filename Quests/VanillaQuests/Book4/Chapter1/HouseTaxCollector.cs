namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseTaxCollector : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.TaxCollector);
}
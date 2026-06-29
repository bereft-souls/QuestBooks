namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseDryad : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Dryad);
}
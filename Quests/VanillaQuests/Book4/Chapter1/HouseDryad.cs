namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseDryad : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Dryad);
}
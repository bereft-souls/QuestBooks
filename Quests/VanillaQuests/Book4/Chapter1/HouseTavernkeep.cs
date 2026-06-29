namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseTavernkeep : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.DD2Bartender);
}
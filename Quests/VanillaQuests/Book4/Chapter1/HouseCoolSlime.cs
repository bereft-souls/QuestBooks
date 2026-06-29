namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseCoolSlime : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.TownSlimeGreen);
}
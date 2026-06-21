namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseCoolSlime : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.TownSlimeGreen);
}
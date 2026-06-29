namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseDivaSlime : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.TownSlimeRainbow);
}
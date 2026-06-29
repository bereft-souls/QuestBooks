namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseNerdySlime : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.TownSlimeBlue);
}
namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseNerdySlime : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.TownSlimeBlue);
}
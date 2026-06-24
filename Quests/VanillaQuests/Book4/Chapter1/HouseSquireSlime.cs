namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseSquireSlime : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.TownSlimeCopper);
}
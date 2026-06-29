namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseSquireSlime : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.TownSlimeCopper);
}
namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseMysticSlime : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.TownSlimeYellow);
}
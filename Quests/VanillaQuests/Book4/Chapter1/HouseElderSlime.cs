namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseElderSlime : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.TownSlimeOld);
}
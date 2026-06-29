namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseElderSlime : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.TownSlimeOld);
}
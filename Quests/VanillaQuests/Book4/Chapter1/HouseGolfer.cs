namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseGolfer : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Golfer);
}
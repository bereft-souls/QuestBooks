namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseClothier : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Clothier);
}
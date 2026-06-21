namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseClothier : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Clothier);
}
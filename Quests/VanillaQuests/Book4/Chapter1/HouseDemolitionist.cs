namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseDemolitionist : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Demolitionist);
}
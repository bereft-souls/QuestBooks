namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HousePartyGirl : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.PartyGirl);
}
namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HousePartyGirl : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.PartyGirl);
}
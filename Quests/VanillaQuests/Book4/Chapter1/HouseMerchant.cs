namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseMerchant : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Merchant);
}
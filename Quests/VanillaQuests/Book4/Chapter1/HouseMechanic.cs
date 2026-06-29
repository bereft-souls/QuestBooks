namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseMechanic : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Mechanic);
}
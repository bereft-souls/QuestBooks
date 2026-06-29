namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HousePirate : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Pirate);
}
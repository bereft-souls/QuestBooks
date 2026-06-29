namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseArmsDealer : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.ArmsDealer);
}
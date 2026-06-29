namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseNurse : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Nurse);
}
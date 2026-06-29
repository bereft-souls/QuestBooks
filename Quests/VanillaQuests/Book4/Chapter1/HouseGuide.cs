namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseGuide : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Guide);
}
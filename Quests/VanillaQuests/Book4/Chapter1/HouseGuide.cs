namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseGuide : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Guide);
}
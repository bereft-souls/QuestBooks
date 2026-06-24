namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HousePrincess : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Princess);
}
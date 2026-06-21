namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseGoblinTinkerer : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.GoblinTinkerer);
}
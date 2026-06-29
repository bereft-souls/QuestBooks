namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseGoblinTinkerer : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.GoblinTinkerer);
}
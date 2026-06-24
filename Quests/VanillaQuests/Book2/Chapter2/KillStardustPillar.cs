namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter2;

public class KillStardustPillar : QBQuest
{
    public override bool CheckCompletion() => NPC.downedTowerStardust;
}
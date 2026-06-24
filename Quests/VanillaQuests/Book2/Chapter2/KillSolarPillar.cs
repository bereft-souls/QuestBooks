namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter2;

public class KillSolarPillar : QBQuest
{
    public override bool CheckCompletion() => NPC.downedTowerSolar;
}
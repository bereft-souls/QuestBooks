namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter2;

public class KillVortexPillar : QBQuest
{
    public override bool CheckCompletion() => NPC.downedTowerVortex;
}
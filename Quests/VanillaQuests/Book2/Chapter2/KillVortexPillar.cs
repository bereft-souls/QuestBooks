namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter2;

public class KillVortexPillar : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedTowerVortex;
}
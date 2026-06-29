namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class ForestBiome : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneForest;
}

using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class ForestBiome : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneForest;
}

using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class MineOre : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class MineOreCheck() : KillTileHook<MineOre>(TileID.Sets.Ore);
}
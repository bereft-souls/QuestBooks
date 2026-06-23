using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class MineOre : QBQuest
{
    public override bool CheckCompletion() => false;

    public class MineOreCheck() : KillTileHook<MineOre>(TileID.Sets.Ore);
}
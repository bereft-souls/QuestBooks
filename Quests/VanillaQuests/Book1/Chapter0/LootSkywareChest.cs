using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class LootSkywareChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class LootSkywareChestCheck() : LootChestHook<LootSkywareChest>(TileID.Containers, ChestFrames.Skyware);
}
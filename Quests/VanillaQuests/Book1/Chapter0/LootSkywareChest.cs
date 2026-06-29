using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class LootSkywareChest : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class LootSkywareChestCheck() : LootChestHook<LootSkywareChest>(TileID.Containers, ChestFrames.Skyware);
}
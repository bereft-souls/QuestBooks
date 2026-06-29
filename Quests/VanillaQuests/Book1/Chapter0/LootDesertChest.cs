using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class LootDesertChest : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class LootDesertChestCheck() : LootChestHook<LootDesertChest>(TileID.Containers2, ChestFrames.Sandstone);
}
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class LootOceanChest : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class LootOceanChestCheck() : LootChestHook<LootOceanChest>(TileID.Containers, ChestFrames.Ocean);
}
using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class LootSurfaceChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class LootSurfaceChestCheck() : LootChestHook<LootSurfaceChest>(TileID.Containers, ChestFrames.Wood, ChestFrames.Gold);
}
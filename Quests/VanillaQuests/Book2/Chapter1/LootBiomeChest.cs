using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class LootBiomeChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class LootBiomeChestCheck() : LootChestHook<LootBiomeChest>
    (
        TileID.Containers,
        ChestFrames.DungeonCorruption,
        ChestFrames.DungeonCrimson, 
        ChestFrames.DungeonHallow, 
        ChestFrames.DungeonJungle, 
        ChestFrames.DungeonTundra
    );
}
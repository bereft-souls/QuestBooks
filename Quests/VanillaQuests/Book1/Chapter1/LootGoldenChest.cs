using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class LootGoldenChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class LootGoldenChestCheck() : LootChestHook<LootGoldenChest>(TileID.Containers, ChestFrames.Gold);
}
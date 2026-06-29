using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class LootJungleChest : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class LootJungleChestCheck() : LootChestHook<LootJungleChest>(TileID.Containers, ChestFrames.Ivy);
}
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class LootDungeonChest : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class LootDungeonChestCheck() : LootChestHook<LootDungeonChest>(TileID.Containers, ChestFrames.LockedGold);
}
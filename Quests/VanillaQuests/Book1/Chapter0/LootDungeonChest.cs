using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class LootDungeonChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class LootDungeonChestCheck() : LootChestHook<LootDungeonChest>(TileID.Containers, ChestFrames.LockedGold);
}
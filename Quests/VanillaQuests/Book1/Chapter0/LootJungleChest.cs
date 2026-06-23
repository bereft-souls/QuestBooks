using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class LootJungleChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class LootJungleChestCheck() : LootChestHook<LootJungleChest>(TileID.Containers, ChestFrames.Ivy);
}
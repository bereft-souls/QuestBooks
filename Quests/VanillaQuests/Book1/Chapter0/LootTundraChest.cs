using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class LootTundraChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class LootTundraChestCheck() : LootChestHook<LootTundraChest>(TileID.Containers, ChestFrames.Ice);
}
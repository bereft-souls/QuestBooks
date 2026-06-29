using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class LootTempleChest : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class LootTempleChestCheck() : LootChestHook<LootTempleChest>(ChestFrames.Lihzahrd);
}
using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class LootTempleChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class LootTempleChestCheck() : LootChestHook<LootTempleChest>(ChestFrames.Lihzahrd);
}
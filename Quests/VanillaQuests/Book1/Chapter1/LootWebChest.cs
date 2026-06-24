using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class LootWebChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class LootWebChestCheck() : LootChestHook<LootWebChest>(TileID.Containers, ChestFrames.Spider);
}
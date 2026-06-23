using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftFurnace : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftFurnaceCheck() : CraftItemHook<CraftFurnace>(ItemID.Furnace);
}
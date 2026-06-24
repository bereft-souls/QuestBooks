using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftFurnace : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftFurnaceCheck() : CraftItemHook<CraftFurnace>(ItemID.Furnace);
}
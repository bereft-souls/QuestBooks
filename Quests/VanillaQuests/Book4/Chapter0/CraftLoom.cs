using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftLoom : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class CraftLoomCheck() : CraftItemHook<CraftLoom>(ItemID.Loom);
}
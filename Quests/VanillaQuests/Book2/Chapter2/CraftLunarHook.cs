using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter2;

public class CraftLunarHook : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftLunarHookCheck() : CraftItemHook<CraftLunarHook>(ItemID.LunarHook);
}
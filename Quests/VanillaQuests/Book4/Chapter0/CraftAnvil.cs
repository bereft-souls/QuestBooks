using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftAnvil : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class CraftAnvilCheck() : CraftItemHook<CraftAnvil>(ItemID.IronAnvil);
}
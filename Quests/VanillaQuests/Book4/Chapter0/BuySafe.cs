using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class BuySafe : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class BuySafeCheck() : BuyItemHook<BuySafe>(ItemID.Safe);
}
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class BuyBugNet : QBQuest
{
    public override bool CheckCompletion() => false;

    public class BuyNetCheck() : BuyItemHook<BuyBugNet>(ItemID.BugNet);
}
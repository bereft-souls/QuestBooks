using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class BuyBugNet : QBQuest
{
    public override bool CheckCompletion() => false;

    public class BuyNetCheck() : BuyItemHook<BuyBugNet>(ItemID.BugNet);
}
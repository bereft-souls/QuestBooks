using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class BuySafe : QBQuest
{
    public override bool CheckCompletion() => false;

    public class BuySafeCheck() : BuyItemHook<BuySafe>(ItemID.Safe);
}
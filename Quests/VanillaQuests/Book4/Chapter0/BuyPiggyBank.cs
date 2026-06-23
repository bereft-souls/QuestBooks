using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class BuyPiggyBank : QBQuest
{
    public override bool CheckCompletion() => false;

    public class BuyPiggyBankCheck() : BuyItemHook<BuyPiggyBank>(ItemID.PiggyBank);
}
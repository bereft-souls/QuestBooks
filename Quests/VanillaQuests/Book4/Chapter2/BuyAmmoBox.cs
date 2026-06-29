using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter2;

public class BuyAmmoBox : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class BuyAmmoBoxCheck() : BuyItemHook<BuyAmmoBox>(ItemID.AmmoBox);
}
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class BuyTinkerersWorkshop : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class BuyTinkerersWorkshopCheck() : BuyItemHook<BuyTinkerersWorkshop>(ItemID.TinkerersWorkshop);
}
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class BuyImbuingStation : QBQuest
{
    public override bool CheckCompletion() => false;

    public class BuyImbuingStationCheck() : BuyItemHook<BuyImbuingStation>(ItemID.ImbuingStation);
}
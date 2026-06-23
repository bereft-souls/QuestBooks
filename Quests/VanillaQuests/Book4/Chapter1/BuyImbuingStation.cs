using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class BuyImbuingStation : QBQuest
{
    public override bool CheckCompletion() => false;

    public class BuyImbuingStationCheck() : BuyItemHook<BuyImbuingStation>(ItemID.ImbuingStation);
}
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class BuyRocketBoots : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class BuyRocketBootsCheck() : BuyItemHook<BuyRocketBoots>(ItemID.RocketBoots);
}
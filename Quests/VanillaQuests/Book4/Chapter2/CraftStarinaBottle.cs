using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter2;

public class CraftStarinaBottle : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class CraftStarinaBottleCheck() : CraftItemHook<CraftStarinaBottle>(ItemID.StarinaBottle);
}
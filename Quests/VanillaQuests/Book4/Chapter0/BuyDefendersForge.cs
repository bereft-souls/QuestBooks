using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class BuyDefendersForge : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class BuyDefendersForgeCheck() : BuyItemHook<BuyDefendersForge>(ItemID.DefendersForge);
}
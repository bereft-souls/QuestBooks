using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class BuyTeleporter : QBQuest
{
    public override bool CheckCompletion() => false;

    public class BuyTeleporterCheck() : BuyItemAmountHook<BuyTeleporter>(ItemID.Teleporter, 2);
}
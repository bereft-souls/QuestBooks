using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftVoidBag : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftVoidBagCheck() : CraftItemHook<CraftVoidBag>(ItemID.ClosedVoidBag);
}
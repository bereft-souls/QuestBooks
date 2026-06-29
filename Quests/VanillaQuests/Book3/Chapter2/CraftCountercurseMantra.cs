using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter2;

public class CraftCountercurseMantra : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftCountercurseMantraCheck() : CraftItemHook<CraftCountercurseMantra>(ItemID.CountercurseMantra);
}
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter2;

public class CraftObsidianShield : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftObsidianShieldCheck() : CraftItemHook<CraftObsidianShield>(ItemID.ObsidianShield);
}
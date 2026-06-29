using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter2;

public class CraftAnkhShield : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftAnkhShieldCheck() : CraftItemHook<CraftAnkhShield>(ItemID.AnkhShield);
}
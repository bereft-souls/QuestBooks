using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class CraftShroomiteDiggingClaws : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftShroomiteDiggingClawsCheck() : CraftItemHook<CraftShroomiteDiggingClaws>(ItemID.ShroomiteDiggingClaw);
}
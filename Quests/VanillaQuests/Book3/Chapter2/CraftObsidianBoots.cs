using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter2;

public class CraftObsidianBoots : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftObsidianBootsCheck() : CraftItemHook<CraftObsidianBoots>(ItemID.ObsidianWaterWalkingBoots);
}
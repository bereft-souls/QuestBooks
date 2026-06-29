using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftBed : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class CraftBedCheck() : CraftItemHook<CraftBed>(ItemID.Bed);
}
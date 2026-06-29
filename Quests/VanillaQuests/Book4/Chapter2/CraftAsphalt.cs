using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter2;

public class CraftAsphalt : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class CraftAsphaltCheck() : CraftItemHook<CraftAsphalt>(ItemID.AsphaltBlock);
}
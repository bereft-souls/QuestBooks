using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftChlorophyteExtractinator : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class CraftChlorophyteExtractinatorCheck() : CraftItemHook<CraftChlorophyteExtractinator>(ItemID.ChlorophyteExtractinator);
}
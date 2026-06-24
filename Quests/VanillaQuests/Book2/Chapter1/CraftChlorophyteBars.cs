using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class CraftChlorophyteBars : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftChlorophyteBarsCheck() : CraftItemHook<CraftChlorophyteBars>(ItemID.ChlorophyteBar);
}
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class CraftSpectreBars : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftSpectreBarsCheck() : CraftItemHook<CraftSpectreBars>(ItemID.SpectreBar);
}
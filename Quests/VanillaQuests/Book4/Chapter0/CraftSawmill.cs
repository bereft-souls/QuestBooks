using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftSawmill : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftSawmillCheck() : CraftItemHook<CraftSawmill>(ItemID.Sawmill);
}
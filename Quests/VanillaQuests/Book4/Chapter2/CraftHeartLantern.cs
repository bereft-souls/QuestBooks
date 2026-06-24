using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter2;

public class CraftHeartLantern : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftHeartLanternCheck() : CraftItemHook<CraftHeartLantern>(ItemID.HeartLantern);
}
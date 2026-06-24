using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter2;

public class CraftReflectiveShades : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftReflectiveShadesCheck() : CraftItemHook<CraftReflectiveShades>(ItemID.ReflectiveShades);
}
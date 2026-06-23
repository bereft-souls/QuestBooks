using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class GetGoldenCarp : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class GetGoldenCarpCheck() : CatchFishHook<GetGoldenCarp>(ItemID.GoldenCarp);
}
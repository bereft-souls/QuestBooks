using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter2;

public class GetAnyBanner : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class GetAnyBannerCheck() : PlaceTileHook<GetAnyBanner>(static (_, _, _, item) => NPCLoader.BannerItemToNPC(item.type) != -1);
}
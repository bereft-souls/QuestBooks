using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter2;

public class GetAnyBanner : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class GetBannersCheck : GlobalTile
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            var banner = NPCLoader.BannerItemToNPC(item.type);

            if (banner == -1)
                return;

            QuestManager.MarkComplete<GetAnyBanner>();
        }
    }
}
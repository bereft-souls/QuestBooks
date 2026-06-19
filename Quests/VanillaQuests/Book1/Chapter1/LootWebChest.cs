using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class LootWebChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class WebChestTileCheck : GlobalTile
    {
        // TODO: Do we want to validate only unexplored chests? Or just any valid chest?
        public override void RightClick(int i, int j, int type)
        {
            if (type != TileID.Containers)
                return;

            var tile = Framing.GetTileSafely(i, j);

            if (tile.TileFrameX != 15 * 36)
                return;

            QuestManager.MarkComplete<LootWebChest>();
        }
    }
}
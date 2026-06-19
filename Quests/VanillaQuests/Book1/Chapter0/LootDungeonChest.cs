using QuestBooks.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class LootDungeonChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class DungeonChestTileCheck : GlobalTile
    {
        // TODO: Do we want to validate only unexplored chests? Or just any valid chest?
        public override void RightClick(int i, int j, int type)
        {
            if (type != TileID.Containers)
            {
                return;
            }
            
            var tile = Framing.GetTileSafely(i, j);

            if (tile.TileFrameX != 0 || tile.TileFrameY != 0)
            {
                return;
            }
            
            QuestManager.MarkComplete<LootDungeonChest>();
        }
    }
}
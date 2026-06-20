using QuestBooks.Chests;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class LootDungeonChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class DungeonChestTileCheck : GlobalTile
    {
        public override void RightClick(int i, int j, int type)
        {
            if (!ChestSystem.IsNatural(i, j) || ChestSystem.IsExplored(i, j))
                return;

            var tile = Framing.GetTileSafely(i, j);

            if (tile.TileFrameX != 0 || tile.TileFrameY != 0)
                return;

            QuestManager.MarkComplete<LootDungeonChest>();
        }
    }
}
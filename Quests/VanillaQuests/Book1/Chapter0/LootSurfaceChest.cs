using QuestBooks.Common.World;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class LootSurfaceChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class LootSurfaceChestCheck : GlobalTile
    {
        public override void RightClick(int i, int j, int type)
        {
            if (!ChestSystem.IsNatural(i, j) || ChestSystem.IsExplored(i, j))
                return;

            var tile = Framing.GetTileSafely(i, j);

            if (tile.TileFrameX != 0)
                return;

            QuestManager.MarkComplete<LootSurfaceChest>();
        }
    }
}
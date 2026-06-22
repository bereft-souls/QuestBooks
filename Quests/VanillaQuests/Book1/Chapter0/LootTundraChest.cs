using QuestBooks.Systems;
using QuestBooks.Systems.Common.World;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class LootTundraChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class LootTundraChestCheck : GlobalTile
    {
        public override void RightClick(int i, int j, int type)
        {
            if (!ChestSystem.IsNatural(i, j) || ChestSystem.IsExplored(i, j))
                return;

            var tile = Framing.GetTileSafely(i, j);

            if (tile.TileFrameX != 11 * 36)
                return;

            QuestManager.MarkComplete<LootTundraChest>();
        }
    }
}
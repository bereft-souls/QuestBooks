using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class LootBiomeChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class LootBiomeChestCheck : GlobalTile
    {
        public override void RightClick(int i, int j, int type)
        {
            if (!ChestSystem.IsNatural(i, j) || ChestSystem.IsExplored(i, j))
                return;

            var tile = Framing.GetTileSafely(i, j);

            if (tile.TileFrameX < 18 * 36 || tile.TileFrameX > 22 * 36)
                return;

            QuestManager.MarkComplete<LootBiomeChest>();
        }
    }
}
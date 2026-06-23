using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class LootGoldenChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class LootGoldenChestCheck : GlobalTile
    {
        public override void RightClick(int i, int j, int type)
        {
            if (!ChestSystem.IsNatural(i, j) || ChestSystem.IsExplored(i, j))
                return;

            var tile = Framing.GetTileSafely(i, j);

            if (tile.TileFrameX != 36)
                return;

            QuestManager.MarkComplete<LootGoldenChest>();
        }
    }
}
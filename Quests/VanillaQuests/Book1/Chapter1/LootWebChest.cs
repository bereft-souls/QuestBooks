using QuestBooks.Systems;
using QuestBooks.World;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class LootWebChest : QBQuest
{
    public override bool CheckCompletion() => false;

    public class LootWebChestCheck : GlobalTile
    {
        public override void RightClick(int i, int j, int type)
        {
            if (!ChestSystem.IsNatural(i, j) || ChestSystem.IsExplored(i, j))
                return;

            var tile = Framing.GetTileSafely(i, j);

            if (tile.TileFrameX != 15 * 36)
                return;

            QuestManager.MarkComplete<LootWebChest>();
        }
    }
}
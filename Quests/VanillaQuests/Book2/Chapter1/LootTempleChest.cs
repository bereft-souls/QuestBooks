using QuestBooks.Common.World;
using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class LootTempleChest : QBQuest
{
    public override bool CheckCompletion() => false;
    
    public class LootTempleChestCheck : GlobalTile
    {
        public override void RightClick(int i, int j, int type)
        {
            if (!ChestSystem.IsNatural(i, j) || ChestSystem.IsExplored(i, j))
                return;
            
            var tile = Framing.GetTileSafely(i, j);

            if (tile.TileFrameX != 16 * 36)
                return;

            QuestManager.MarkComplete<LootTempleChest>();
        }
    }
}
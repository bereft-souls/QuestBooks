using QuestBooks.Systems;
using System.Linq;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class ChopTree : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class TreeTileCheck : GlobalTile
    {
        public override void Drop(int i, int j, int type)
        {
            if (type != TileID.Trees && !(ModContent.GetModTile(type)?.AdjTiles.Contains(TileID.Trees) ?? false))
                return;

            QuestManager.CompleteQuest<ChopTree>();
        }
    }
}
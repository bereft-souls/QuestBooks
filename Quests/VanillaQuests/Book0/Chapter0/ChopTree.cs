using QuestBooks.Systems;
using System.Linq;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class ChopTree : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public sealed class TreeTileCheck : GlobalTile
    {
        public override void Drop(int i, int j, int type)
        {
            if (type != TileID.Trees && !(ModContent.GetModTile(type)?.AdjTiles.Contains(TileID.Trees) ?? false))
                return;

            QuestBooksMod.CompleteQuest<ChopTree>();
        }
    }
}
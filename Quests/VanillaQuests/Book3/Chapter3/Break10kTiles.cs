using QuestBooks.Systems;
using Terraria.ObjectData;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class Break10kTiles : QBQuest
{
    /// <summary>
    ///     The amount of tiles the player must break in order to complete the quest.
    /// </summary>
    public const int TargetTiles = 10000;

    public int TilesBroken { get; private set; }

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => TilesBroken >= TargetTiles;

    private sealed class TileBreakGlobalTile : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (Main.dedServ)
                return;

            if (fail || effectOnly || (Main.tileFrameImportant[type] && !TileObjectData.IsTopLeft(i, j)))
                return;

            QuestManager.GetQuest<Break10kTiles>().TilesBroken++;
        }
    }
}
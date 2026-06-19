using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class CrumbleDungeonBricks : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CrumbleDungeonBricksTileCheck : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (type != TileID.CrackedBlueDungeonBrick && type != TileID.CrackedGreenDungeonBrick && type != TileID.CrackedPinkDungeonBrick)
                return;

            QuestManager.MarkComplete<CrumbleDungeonBricks>();
        }
    }
}
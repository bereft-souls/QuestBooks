using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class CrumbleDungeonBricks : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CrumbleDungeonBricksCheck() : KillTileHook<CrumbleDungeonBricks>(TileID.CrackedBlueDungeonBrick, TileID.CrackedGreenDungeonBrick, TileID.CrackedPinkDungeonBrick);
}
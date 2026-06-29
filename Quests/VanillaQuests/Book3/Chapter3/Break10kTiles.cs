using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class Break10kTiles : VanillaQuest
{
    /// <summary>
    ///     The amount of tiles the player must break in order to complete the quest.
    /// </summary>
    public const int TargetTiles = 10000;

    public int TilesBroken { get; private set; }

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => TilesBroken >= TargetTiles;

    public sealed class KillAnyTileCheck() : KillTileHook((_, _, _) => QuestManager.GetQuest<Break10kTiles>().TilesBroken++);
}
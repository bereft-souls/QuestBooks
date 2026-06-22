using QuestBooks.Common.Tiles;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class Break10kTiles : QBQuest
{
    /// <summary>
    ///     The amount of tiles the player must break in order to complete the quest.
    /// </summary>
    public const int Target = 10000;

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.GetModPlayer<TileBreakPlayer>().Count >= Target;
}
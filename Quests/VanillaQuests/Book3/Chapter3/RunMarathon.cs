using QuestBooks.Systems.Common.Movement;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class RunMarathon : QBQuest
{
    /// <summary>
    ///     The number of tiles the player must run in order to complete the quest.
    /// </summary>
    public const int Target = 69200;

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.GetModPlayer<MovementTrackerPlayer>().Tiles >= Target;
}
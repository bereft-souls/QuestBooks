using QuestBooks.Common.Shimmer;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class ShimmerPhase : QBQuest
{
    /// <summary>
    ///     The amount of tiles the player must phase through while shimmering in order to complete the quest.
    /// </summary>
    public const int Target = 200;

    private int start;

    private int end;
    
    public override QuestType QuestType => QuestType.Player;

    public override void Load()
    {
        ShimmerCallbacks.OnStartPhase += Start;
        ShimmerCallbacks.OnEndPhase += End;
    }

    public override bool CheckCompletion() => Math.Abs(end - start) >= Target;

    private void Start(Player player) => start = player.Center.ToTileCoordinates().Y;
    
    private void End(Player player) => end = player.Center.ToTileCoordinates().Y;
}
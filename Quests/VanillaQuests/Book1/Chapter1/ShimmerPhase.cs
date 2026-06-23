using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class ShimmerPhase : QBQuest
{
    /// <summary>
    ///     The amount of tiles the player must phase through while shimmering in order to complete the
    ///     quest.
    /// </summary>
    public const int Target = 200;

    private int start;
    private int end;

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Math.Abs(end - start) >= Target;

    public void Start(Player player) => start = player.Center.ToTileCoordinates().Y;

    public void End(Player player) => end = player.Center.ToTileCoordinates().Y;

    public sealed class ShimmerPhaseCheck : ModPlayer
    {
        private bool flag;
        
        public override void PostUpdate()
        {
            var shimmering = Player.shimmering;

            if (shimmering && !flag)
                QuestManager.GetQuest<ShimmerPhase>().Start(Player);

            if (!shimmering && flag)
                QuestManager.GetQuest<ShimmerPhase>().End(Player);

            flag = shimmering;
        }
    }
}
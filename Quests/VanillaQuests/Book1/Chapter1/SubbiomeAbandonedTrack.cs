
namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class SubbiomeAbandonedTrack : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.InModBiome<AbandonedTrackBiome>();

    public sealed class AbandonedTrackBiome : ModBiome
    {
        public override bool IsBiomeActive(Player player) => AbandonedTrackBiomeSystem.Active && (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight);
    }

    public sealed class AbandonedTrackBiomeSystem : ModSystem
    {
        /// <summary>
        ///     Defines the minimum count of abandoned track tiles required for the player to be considered
        ///     inside
        ///     an abandoned track.
        /// </summary>
        public const int Threshold = 50;

        /// <summary>
        ///     Gets the current count of abandoned track tiles detected around the player.
        /// </summary>
        public static int Count { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the player is inside an abandoned track.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if <see cref="Count"/> is greater than or equal to
        ///     <see cref="Threshold"/>; otherwise, <see langword="false"/>.
        /// </value>
        public static bool Active => Count >= Threshold;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            Count = tileCounts[TileID.MinecartTrack];
        }
    }
}
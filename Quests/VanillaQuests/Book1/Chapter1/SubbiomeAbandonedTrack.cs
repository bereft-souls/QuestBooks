namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class SubbiomeAbandonedTrack : QBQuest
{
    public override bool CheckCompletion() => SubbiomeAbandonedTrackCheck.Active && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight);

    // TODO: Possibly repurpose this as a common system for greater reusability across the mod.
    public class SubbiomeAbandonedTrackCheck : ModSystem
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
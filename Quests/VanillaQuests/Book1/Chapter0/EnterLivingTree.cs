namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class EnterLivingTree : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.InModBiome<LivingTreeBiome>();

    public sealed class LivingTreeBiome : ModBiome
    {
        public override bool IsBiomeActive(Player player) => LivingTreeBiomeSystem.Active && player.ZoneOverworldHeight;
    }

    public sealed class LivingTreeBiomeSystem : ModSystem
    {
        /// <summary>
        ///     Defines the minimum count of living tree tiles required for the player to be considered inside
        ///     the living tree biome.
        /// </summary>
        public const int Threshold = 100;

        /// <summary>
        ///     Gets the current count of living tree tiles detected around the player.
        /// </summary>
        public static int Count { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the player is inside the living tree biome.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if <see cref="Count"/> is greater than or equal to
        ///     <see cref="Threshold"/>; otherwise, <see langword="false"/>.
        /// </value>
        public static bool Active => Count >= Threshold;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            Count = tileCounts[TileID.LivingMahoganyLeaves] + tileCounts[TileID.LivingMahogany];
        }
    }
}
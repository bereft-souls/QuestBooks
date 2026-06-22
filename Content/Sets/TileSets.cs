namespace QuestBooks.Content.Sets;

public static class TileSets
{
    /// <summary>
    ///     Contains sets of tiles that are categorized as ores.
    /// </summary>
    public static class Ores
    {
        /// <summary>
        ///     A set containing all hardmode ores in the game.
        /// </summary>
        public static readonly ContentSet Hardmode = new()
        {
            TileID.Cobalt,
            TileID.Palladium,
            TileID.Mythril,
            TileID.Orichalcum,
            TileID.Adamantite,
            TileID.Titanium
        };
    }
}
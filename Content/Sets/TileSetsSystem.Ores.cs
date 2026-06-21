namespace QuestBooks.Content.Sets;

public sealed partial class TileSetsSystem
{
    public static class Ores
    {
        public static bool[] Hardmode { get; internal set; }

        public static bool[] Any => TileID.Sets.Ore;
    }

    private static void SetupOres()
    {
        Ores.Hardmode = TileID.Sets.Factory.CreateBoolSet
        (
            TileID.Cobalt,
            TileID.Palladium,
            TileID.Mythril,
            TileID.Orichalcum,
            TileID.Adamantite,
            TileID.Titanium
        );
    }
}
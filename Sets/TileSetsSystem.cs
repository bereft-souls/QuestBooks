namespace QuestBooks.Sets;

public sealed class TileSetsSystem : ModSystem
{
    public static class Ores
    {
        public static bool[] Hardmode { get; internal set; }

        public static bool[] Any => TileID.Sets.Ore;
    }

    public override void PostSetupContent()
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
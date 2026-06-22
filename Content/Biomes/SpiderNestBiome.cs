namespace QuestBooks.Content.Biomes;

public sealed class SpiderNestBiome : ModBiome
{
    // TODO: There's probably a better way to check this. Investigate.
    public override bool IsBiomeActive(Player player) => Framing.GetTileSafely(player.position.ToTileCoordinates()).WallType == WallID.SpiderUnsafe;
}
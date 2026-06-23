namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class SubbiomeSpiders : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.InModBiome<SpiderNestBiome>();

    public sealed class SpiderNestBiome : ModBiome
    {
        // TODO: There's probably a better way to check this. Investigate.
        public override bool IsBiomeActive(Player player) => Framing.GetTileSafely(player.position.ToTileCoordinates()).WallType == WallID.SpiderUnsafe;
    }
}
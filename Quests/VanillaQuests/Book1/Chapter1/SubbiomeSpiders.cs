namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class SubbiomeSpiders : QBQuest
{
    // TODO: There's probably a better way to check this. Investigate.
    public override bool CheckCompletion() => Framing.GetTileSafely(Main.LocalPlayer.position.ToTileCoordinates()).WallType == WallID.SpiderUnsafe;
}
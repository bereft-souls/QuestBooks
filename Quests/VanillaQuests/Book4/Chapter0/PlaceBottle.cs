using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class PlaceBottle : QBQuest
{
    public override bool CheckCompletion() => false;
    
    public class PlaceBottleCheck() : PlaceTileHook(Complete)
    {
        private static void Complete(int i, int j, int type, Item item)
        {
            if (type != TileID.Bottles)
                return;

            var bottom = Framing.GetTileSafely(i, j + 1);

            if (!TileID.Sets.Platforms[bottom.TileType])
                return;

            QuestManager.MarkComplete<PlaceBottle>();
        }
    }
}
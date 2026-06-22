using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter2;

public class BuildPlatforms : QBQuest
{
    public override bool CheckCompletion() => false;

    public class BuildPlatformsCheck : GlobalTile
    {
        /// <summary>
        ///     The maximum vertical distance between platform rows for them to count towards the quest.
        /// </summary>
        public const int Tolerance = 25;
        
        /// <summary>
        ///     The number of platforms the player must place in a row in order to complete the quest.
        /// </summary>
        public const int Target = 75;
        
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            if (!TileID.Sets.Platforms[type])
                return;

            for (var k = 1; k <= Tolerance; k++)
            {
                var above = CountRow(i, j + k);
                var below = CountRow(i, j - k);

                if (above < Target || below < Target)
                    continue;
                
                QuestManager.MarkComplete<BuildPlatforms>();
            }
        }
        
        private static int CountPlatforms(int x, int y, int direction)
        {
            var count = 0;

            for (var i = 0; i < Target; i++)
            {
                var tile = Framing.GetTileSafely(x + direction * i, y);

                if (TileID.Sets.Platforms[tile.TileType])
                    count++;
                else
                    break;
            }

            return count;
        }

        private static int CountRow(int x, int y) => CountPlatforms(x, y, 1) + CountPlatforms(x, y, -1);
    }
}
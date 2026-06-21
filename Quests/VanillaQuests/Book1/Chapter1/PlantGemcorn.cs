using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class PlantGemcorn : QBQuest
{
    public override bool CheckCompletion() => false;

    public class PlantGemcornCheck : GlobalTile
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            if (type != TileID.GemSaplings)
                return;

            QuestManager.MarkComplete<PlantGemcorn>();
        }
    }
}
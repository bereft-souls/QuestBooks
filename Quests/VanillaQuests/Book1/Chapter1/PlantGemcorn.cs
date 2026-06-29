using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class PlantGemcorn : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class PlantGemcornCheck() : PlaceTileHook<PlantGemcorn>(TileID.GemSaplings);
}
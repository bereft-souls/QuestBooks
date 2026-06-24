using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class GetWaterCandle : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class GetWaterCandleCheck() : KillTileHook<GetWaterCandle>(TileID.WaterCandle);
}
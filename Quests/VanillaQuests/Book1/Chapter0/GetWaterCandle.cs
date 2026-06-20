using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class GetWaterCandle : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class GetWaterCandleCheck : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail || noItem || type != TileID.WaterCandle)
                return;
            
            QuestManager.MarkComplete<GetWaterCandle>();
        }
    }
}
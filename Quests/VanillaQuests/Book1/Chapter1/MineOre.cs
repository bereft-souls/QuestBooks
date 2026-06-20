using QuestBooks.Sets;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class MineOre : QBQuest
{
    public override bool CheckCompletion() => false;

    public class OreTileCheck : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail || !TileSetsSystem.Ores.Any[type])
            {
                return;
            }
            
            QuestManager.MarkComplete<MineOre>();
        }
    }
}
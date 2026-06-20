using QuestBooks.Sets;
using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class MineHMOre : QBQuest
{
    public override bool CheckCompletion() => false;
    
    public class HMOreTileCheck : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail || !TileSetsSystem.Ores.Hardmode[type])
            {
                return;
            }
            
            QuestManager.MarkComplete<MineHMOre>();
        }
    }
}
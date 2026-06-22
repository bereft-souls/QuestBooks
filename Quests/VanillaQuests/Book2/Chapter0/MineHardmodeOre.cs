using QuestBooks.Content.Sets;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class MineHardmodeOre : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class MineHMOreCheck : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail || noItem || !TileSets.Ores.Hardmode[type])
                return;

            QuestManager.MarkComplete<MineHardmodeOre>();
        }
    }
}
using QuestBooks.Content.Sets;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class MineOre : QBQuest
{
    public override bool CheckCompletion() => false;

    public class MineOreCheck : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail || effectOnly || noItem || !TileID.Sets.Ore[type])
                return;

            QuestManager.MarkComplete<MineOre>();
        }
    }
}
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class SmashEvilAltar : QBQuest
{
    public override bool CheckCompletion() => false;

    public class SmashEvilAltarCheck : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (type != TileID.DemonAltar)
                return;

            QuestManager.MarkComplete<SmashEvilAltar>();
        }
    }
}
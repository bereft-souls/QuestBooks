using QuestBooks.Systems;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class SmashEvilCore : QBQuest
{
    public override bool CheckCompletion() => false;
    
    public class EvilCoreTileCheck : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (type != TileID.ShadowOrbs || fail)
            {
                return;
            }
            
            QuestManager.MarkComplete<SmashEvilCore>();
        }
    }
}
using QuestBooks.Systems;
using Terraria.GameContent.Events;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class SetupOldOnesArmy : QBQuest
{
    public override bool CheckCompletion() => false;

    public class SetupOldOnesArmyCheck : GlobalTile
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            if (type != TileID.ElderCrystalStand || DD2Event.WouldFailSpawningHere(i, j))
                return;

            QuestManager.MarkComplete<SetupOldOnesArmy>();
        }
    }
}
using QuestBooks.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class SolarEclipseDefeated : QBQuest
{
    public override bool CheckCompletion() => false;

    public class SolarEclipseCheck : ModSystem
    {
        private bool cachedEclipse = false;

        public override void PostUpdateTime()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            if (!Main.eclipse && cachedEclipse)
                QuestManager.CompleteQuest<SolarEclipseDefeated>();

            cachedEclipse = Main.eclipse;
        }
    }
}

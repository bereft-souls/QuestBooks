using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class SolarEclipseDefeated : VanillaQuest
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
                QuestBooksMod.CompleteQuest<SolarEclipseDefeated>();

            cachedEclipse = Main.eclipse;
        }
    }
}

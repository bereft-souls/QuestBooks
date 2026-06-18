using QuestBooks.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class BloodMoonDefeated : QBQuest
{
    public override bool CheckCompletion() => false;

    public class BloodMoonCheck : ModSystem
    { 
        private bool cachedBloodMoon = false;

        public override void PostUpdateTime()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            if (!Main.bloodMoon && cachedBloodMoon)
                QuestManager.CompleteQuest<BloodMoonDefeated>();

            cachedBloodMoon = Main.bloodMoon;
        }
    }
}

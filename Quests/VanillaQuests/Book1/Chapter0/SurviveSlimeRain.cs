using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class SurviveSlimeRain : QBQuest
{
    public override bool CheckCompletion() => false;

    [Autoload(Side = ModSide.Client)]
    public class SlimeRainCheck : ModSystem
    {
        private static bool cachedSlimeRain;

        public override void PostUpdateTime()
        {
            if (!Main.slimeRain && cachedSlimeRain)
                QuestManager.CompleteQuest<SurviveSlimeRain>();

            cachedSlimeRain = Main.slimeRain;
        }
    }
}
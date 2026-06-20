using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class SurviveSlimeRain : QBQuest
{
    public override bool CheckCompletion() => false;

    public class SurviveSlimeRainCheck : ModSystem
    {
        public override void Load() => On_Main.StopSlimeRain += Check;

        private static void Check(On_Main.orig_StopSlimeRain orig, bool announce)
        {
            orig(announce);
            
            QuestManager.MarkComplete<SurviveSlimeRain>();
        }
    }
}
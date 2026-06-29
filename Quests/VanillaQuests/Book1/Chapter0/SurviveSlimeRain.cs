using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class SurviveSlimeRain : VanillaQuest
{
    public override void Load() => On_Main.StopSlimeRain += Check;

    public override bool CheckCompletion() => false;

    private static void Check(On_Main.orig_StopSlimeRain orig, bool announce)
    {
        orig(announce);
        QuestBooksMod.MarkComplete<SurviveSlimeRain>();
    }
}
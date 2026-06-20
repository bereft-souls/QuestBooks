using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class AlertMartiansProbe : QBQuest
{
    public override bool CheckCompletion() => false;

    public class AlertMartiansProbeCheck : ModSystem
    {
        public override void Load() => On_Main.StartInvasion += Check;

        private static void Check(On_Main.orig_StartInvasion orig, int type)
        {
            orig(type);

            if (type != InvasionID.MartianMadness)
                return;
            
            QuestManager.MarkComplete<AlertMartiansProbe>();
        }
    }
}
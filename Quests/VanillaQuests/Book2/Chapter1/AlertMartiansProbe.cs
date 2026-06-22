using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class AlertMartiansProbe : QBQuest
{
    public override void Load() => On_Main.StartInvasion += Check;
    
    public override bool CheckCompletion() => false;
    
    private static void Check(On_Main.orig_StartInvasion orig, int type)
    {
        orig(type);

        if (type != InvasionID.MartianMadness)
            return;

        QuestManager.MarkComplete<AlertMartiansProbe>();
    }
}
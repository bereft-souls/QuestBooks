using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria.GameContent.Achievements;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class DefeatMechaMayhem : QBQuest
{
    public override void Load() => AchievementsHelper.OnProgressionEvent += Check;

    public override bool CheckCompletion() => false;
    
    private static void Check(int eventId)
    {
        if (eventId != AchievementHelperID.Events.DefeatedMechaMayhem)
            return;
        
        QuestManager.MarkComplete<DefeatMechaMayhem>();
    }
}
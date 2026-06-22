using QuestBooks.Systems;
using Terraria.GameContent.Achievements;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class SurviveLongFall : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override void Load() => On_AchievementsHelper.HandleSpecialEvent += On_AchievementsHelperOnHandleSpecialEvent;

    public override bool CheckCompletion() => false;
    
    private void On_AchievementsHelperOnHandleSpecialEvent(On_AchievementsHelper.orig_HandleSpecialEvent orig, Player player, int eventId)
    {
        orig(player, eventId);

        if (eventId != AchievementHelperID.Special.SurviveHugeFall)
            return;
        
        QuestManager.MarkComplete<SurviveLongFall>();
    }
}
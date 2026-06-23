using QuestBooks.Systems;
using Terraria.GameContent.Achievements;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class SurviveLongFall : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override void Load() => On_AchievementsHelper.HandleSpecialEvent += Check;

    public override bool CheckCompletion() => false;

    private static void Check(On_AchievementsHelper.orig_HandleSpecialEvent orig, Player player, int eventId)
    {
        if (eventId == AchievementHelperID.Special.SurviveHugeFall)
        {
            QuestManager.MarkComplete<SurviveLongFall>();
        }
        
        orig(player, eventId);
    }
}
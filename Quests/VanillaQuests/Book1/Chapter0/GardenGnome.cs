using QuestBooks.Systems;
using Terraria.GameContent.Achievements;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class GardenGnome : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override void Load() => On_AchievementsHelper.NotifyProgressionEvent += Check;

    public override bool CheckCompletion() => false;

    private static void Check(On_AchievementsHelper.orig_NotifyProgressionEvent orig, int eventId)
    {
        if (eventId == AchievementHelperID.Events.TurnGnomeToStatue)
            QuestBooksMod.MarkComplete<GardenGnome>();

        orig(eventId);
    }
}
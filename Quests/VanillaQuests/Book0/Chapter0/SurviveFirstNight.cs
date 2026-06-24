using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class SurviveFirstNight : QBQuest
{
    public bool CachedNight { get; set; } = false;

    public override bool CheckCompletion() => false;

    public override void Update()
    {
        bool day = Main.IsItDay();

        if (day && CachedNight)
            QuestManager.CompleteQuest<SurviveFirstNight>();

        CachedNight = !day;
    }
}

using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class SurviveFirstNight : VanillaQuest
{
    public bool CachedNight { get; set; } = false;

    public override bool CheckCompletion() => false;

    public override void Update()
    {
        bool day = Main.IsItDay();

        if (day && CachedNight)
            QuestBooksMod.CompleteQuest<SurviveFirstNight>();

        CachedNight = !day;
    }
}

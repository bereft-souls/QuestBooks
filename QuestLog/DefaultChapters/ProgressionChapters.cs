using QuestBooks.Quests.VanillaQuests.Book0.Chapter0;
using QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;
using QuestBooks.Systems;

namespace QuestBooks.QuestLog.DefaultChapters
{
    public class PostIntroChapter : ScrollChapter
    {
        public override bool IsUnlocked() => QuestManager.GetQuest<BasicsCompleteInfo>().Completed;
    }

    public class HardmodeChapter : ScrollChapter
    {
        public override bool IsUnlocked() => QuestManager.GetQuest<WallOfFleshDefeated>().Completed;
    }

    public class EndgameChapter : ScrollChapter
    {
        public override bool IsUnlocked() => QuestManager.GetQuest<LunaticCultistDefeated>().Completed;
    }
}

using QuestBooks.QuestLog.DefaultStyles;
using System.Text;

namespace QuestBooks.Quests
{
    public static class VanillaQuestBooks
    {
        public static void AddVanillaQuests()
        {
            QuestBooksMod.AddQuestLogStyle(new BasicQuestLogStyle(), QuestBooksMod.Instance);

            var questLogBytes = QuestBooksMod.Instance.GetFileBytes("Quests/VanillaQuestLog.json");
            var questLogString = Encoding.UTF8.GetString(questLogBytes);

            QuestBooksMod.AddQuestLog("Vanilla", questLogString);
        }
    }
}

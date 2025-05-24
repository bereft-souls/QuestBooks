using QuestBooks.QuestLog.DefaultQuestBooks;
using QuestBooks.QuestLog.DefaultQuestLines;
using System.Linq;

namespace QuestBooks.Quests
{
    public static class VanillaQuests
    {
        public static readonly string[] Books = [

        ];

        public static void AddVanillaQuests()
        {
            int testBookCount = 6;
            int maxQuestLines = 10;

            for (int i = 0; i < testBookCount; i++)
            {
                string bookKeys = QuestBooks.Instance.GetLocalizationKey($"VanillaQuests.TestBook{i + 1}");

                var basicBook = new BasicQuestBook()
                {
                    NameKey = $"{bookKeys}.DisplayName",
                    QuestLines = [..Enumerable.Range(0, maxQuestLines - i).Select(num => new BasicQuestLine() { NameKey = $"{bookKeys}.TestChapter{num + 1}" })]
                };

                QuestBooks.AddQuestBook(basicBook, QuestBooks.Instance);
            }

            return;

            foreach (string book in Books)
                QuestBooks.Instance.Call("addquestbook", book, QuestBooks.Instance);
        }
    }
}

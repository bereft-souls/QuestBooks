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
                string bookKeys = QuestBooksMod.Instance.GetLocalizationKey($"VanillaQuests.TestBook{i + 1}");

                var basicBook = new BasicQuestBook()
                {
                    NameKey = $"{bookKeys}.DisplayName",
                    QuestLines = [..Enumerable.Range(0, maxQuestLines - i).Select(num => new BasicQuestLine() { NameKey = $"{bookKeys}.TestChapter{num + 1}" })]
                };

                QuestBooksMod.AddQuestBook(basicBook, QuestBooksMod.Instance);
            }

            return;

            foreach (string book in Books)
                QuestBooksMod.Instance.Call("addquestbook", book, QuestBooksMod.Instance);
        }
    }
}

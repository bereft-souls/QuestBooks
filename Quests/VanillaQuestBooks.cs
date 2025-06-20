using QuestBooks.QuestLog.DefaultQuestBooks;
using QuestBooks.QuestLog.DefaultQuestLines;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QuestBooks.Quests
{
    public static class VanillaQuestBooks
    {
        public static void AddVanillaQuests()
        {
            const string questFolder = "Quests/VanillaBooks/";

            // Get all .json files in the vanilla questbooks folder and order them alphanumerically
            var questBookFiles = QuestBooksMod.Instance.GetFileNames().Where(x => x.StartsWith(questFolder) && x.EndsWith(".json")).OrderBy(x => x.Replace(questFolder, ""));

            // Retrieve and parse each file into it's string representation
            var questBooks = questBookFiles.Select(x => Encoding.UTF8.GetString(QuestBooksMod.Instance.GetFileBytes(x)));

            // Load each book
            foreach (var questBook in questBooks)
                QuestBooksMod.AddQuestBook(questBook, QuestBooksMod.Instance);
        }
    }
}

using QuestBooks.QuestLog.DefaultStyles;
using System.Text;
using Terraria.ModLoader;

namespace QuestBooks.Quests
{
    public static class VanillaQuestBooks
    {
        public static void AddVanillaQuests(Mod mod)
        {
            QuestBooksMod.AddQuestLogStyle(new BasicQuestLogStyle(), mod);

            var questLogBytes = mod.GetFileBytes("Quests/VanillaQuestLog.json");
            var questLogString = Encoding.UTF8.GetString(questLogBytes);

            QuestBooksMod.AddQuestLog("Terraria", questLogString, mod);
        }
    }
}

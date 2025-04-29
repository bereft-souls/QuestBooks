using QuestBooks.QuestLog;
using QuestBooks.Quests;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace QuestBooks.Systems
{
    internal class QuestLoader : ModSystem
    {
        private const string TagKey = "QuestBooks:CompletedQuests";

        // We use a separate dictionary for loading and accessing to
        // improve performance and ensure that quests are not modified post-setup.
        private static Dictionary<string, Type> loadingQuests = [];
        public static FrozenDictionary<Type, string> QuestNames { get; internal set; }

        public static void LoadQuests(QuestBook questBook)
        {
            var loadingAssembly = questBook.GetType().Assembly;
            var questTypes = loadingAssembly.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(Quest)));

            foreach (var questType in questTypes)
            {
                var quest = (Quest)Activator.CreateInstance(questType);
                loadingQuests.TryAdd(quest.Key, questType);
            }
        }

        // Mods should load their quests in PostSetupContent().
        // Following that, we freeze the dictionary and reset the loading.
        public override void OnModLoad()
        {
            QuestNames = loadingQuests.Select(kvp => new KeyValuePair<Type, string>(kvp.Value, kvp.Key)).ToFrozenDictionary();
            loadingQuests = [];
        }

        // This is the first loading related method called on world entry.
        // We use it to reset which quests are considered "complete".
        public override void OnWorldLoad() => QuestManager.LoadActiveQuests();

        #region Quest Loading

        public override void LoadWorldData(TagCompound tag)
        {
            // Attempt to fetch completed quest data from world.
            // If it does not exist, leave all quests as incomplete.
            if (!tag.TryGet(TagKey, out string[] quests))
                return;

            LoadCompletedQuests(quests);
        }

        public partial class PlayerQuestLoader : ModPlayer
        {
            public override void LoadData(TagCompound tag)
            {
                // Attempt to fetch completed quest data from player.
                // If it does not exist, leave all player quests as incomplete.
                if (!tag.TryGet(TagKey, out string[] quests))
                    return;

                LoadCompletedQuests(quests);
            }
        }

        #endregion

        #region Quest Saving

        public override void SaveWorldData(TagCompound tag) => tag[TagKey] = QuestManager.CompletedWorldQuests.ToList();

        public partial class PlayerQuestLoader : ModPlayer
        {
            public override void SaveData(TagCompound tag) => tag[TagKey] = QuestManager.CompletedPlayerQuests.ToList();
        }

        #endregion

        public override void OnWorldUnload() => QuestManager.UnloadActiveQuests();

        public static void LoadCompletedQuests(IEnumerable<string> completedQuests)
        {
            foreach (string quest in completedQuests)
                QuestManager.MarkComplete(quest);
        }
    }
}

using QuestBooks.QuestLog;
using QuestBooks.Quests;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.IO;

namespace QuestBooks.Systems
{
    internal class QuestLoader : ModSystem
    {
        private const string TagKey = "QuestBooks:CompletedQuests";

        // We use a separate dictionary for loading and accessing to
        // improve performance and ensure that quests are not modified post-setup.
        private static readonly Dictionary<string, Type> loadingQuests = [];
        private static readonly List<Assembly> checkedAssemblies = [];
        public static FrozenDictionary<Type, string> QuestNames { get; internal set; }

        public static QuestLogStyle ExclusiveOverrideStyle = null;
        public static Dictionary<Mod, List<QuestLogStyle>> LogStyleRegistry = [];

        public static void LoadQuests(Mod mod)
        {
            var loadingAssembly = mod.Code;

            if (checkedAssemblies.Contains(loadingAssembly))
                return;

            checkedAssemblies.Add(loadingAssembly);
            var questTypes = AssemblyManager.GetLoadableTypes(loadingAssembly).Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(Quest)));

            foreach (var questType in questTypes)
            {
                var quest = (Quest)Activator.CreateInstance(questType);
                loadingQuests.TryAdd(quest.Key, questType);
            }
        }

        // Mods should load their quests in PostSetupContent().
        // Following that, we freeze the dictionary and reset the loading.
        public override void PostAddRecipes()
        {
            QuestNames = loadingQuests.Select(kvp => new KeyValuePair<Type, string>(kvp.Value, kvp.Key)).ToFrozenDictionary();
            loadingQuests.Clear();
            checkedAssemblies.Clear();

            //if (QuestManager.QuestBooks.Count == 0)
            //    VanillaQuests.AddVanillaQuests();

            QuestManager.QuestLogStyles = LogStyleRegistry
                .SelectMany(kvp => kvp.Value)
                .Select(q => new KeyValuePair<string, QuestLogStyle>(q.Key, q))
                .ToDictionary();
        }

        public override void PostSetupRecipes()
        {
            if (ExclusiveOverrideStyle != null)
                QuestManager.ActiveStyle = ExclusiveOverrideStyle;

            // TODO: Change this for config loading
            else
                QuestManager.ActiveStyle = QuestManager.QuestLogStyles.First().Value;

            QuestManager.ActiveStyle.OnSelect();
        }

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

        // This is the first loading related method called on world entry.
        // We use it to reset which quests are considered "complete".
        public override void OnWorldLoad() => QuestManager.LoadActiveQuests();

        public override void OnWorldUnload() => QuestManager.UnloadActiveQuests();

        public static void LoadCompletedQuests(IEnumerable<string> completedQuests)
        {
            foreach (string quest in completedQuests)
                QuestManager.MarkComplete(quest);
        }
    }
}

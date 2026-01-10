using MonoMod.Utils;
using Newtonsoft.Json;
using QuestBooks.QuestLog;
using QuestBooks.QuestLog.DefaultLogStyles;
using QuestBooks.Quests;
using QuestBooks.Utilities;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
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
        private static readonly HashSet<Assembly> checkedAssemblies = [];
        internal static readonly Dictionary<Type, string> loadingQuests = [];
        public static FrozenDictionary<Type, string> QuestKeys { get; internal set; }

        public static QuestLogStyle ExclusiveOverrideStyle = null;
        public static Dictionary<Mod, List<QuestLogStyle>> LogStyleRegistry = [];

        internal static void LoadQuests(Mod mod)
        {
            var loadingAssembly = mod.Code;

            if (checkedAssemblies.Contains(loadingAssembly))
                return;

            checkedAssemblies.Add(loadingAssembly);
            var types = AssemblyManager.GetLoadableTypes(loadingAssembly);
            //var questTypes = types.Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(Quest)));

            QuestManager.AvailableQuestBookTypes.AddRange(types.Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(QuestBook))).OrderBy(t => t.Name));
            QuestManager.AvailableQuestLineTypes.AddRange(types.Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(BookChapter))).OrderBy(t => t.Name));

            QuestManager.AvailableQuestElementTypes.AddRange(types.Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(ChapterElement)))
                .Select(t => new KeyValuePair<Type, ChapterElement>(t, (ChapterElement)Activator.CreateInstance(t)))
                .Select(kvp => { kvp.Value.TemplateInstance = true; return kvp; })
                .OrderBy(kvp => kvp.Key.Name)
                .ToDictionary());

            //foreach (var questType in questTypes)
            //{
            //    var quest = (Quest)Activator.CreateInstance(questType);
            //    loadingQuests.TryAdd(quest.Key, questType);
            //}
        }

        // Mods should load their quests in PostSetupContent().
        // Following that, we freeze the dictionary and reset the loading.
        public override void PostAddRecipes()
        {
            QuestKeys = loadingQuests.ToFrozenDictionary();
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

            QuestManager.SelectQuestLog(QuestManager.AvailableQuestLogs.First().Key);
        }

        #region Quest Loading

        public override void LoadWorldData(TagCompound tag)
        {
            // Attempt to fetch completed quest data from world.
            // If it does not exist, leave all quests as incomplete.
            if (tag.TryGet(TagKey, out string[] quests))
                LoadCompletedQuests(quests);

            // Immediately mark any quests that should have previously been completed
            foreach (var quest in QuestManager.IncompleteWorldQuests.Select(QuestManager.GetQuest).ToArray())
            {
                if (quest.CheckCompletion())
                    QuestManager.MarkComplete(quest);
            }
        }

        public partial class PlayerQuestLoader : ModPlayer
        {
            public override void LoadData(TagCompound tag)
            {
                // Attempt to fetch completed quest data from player.
                // If it does not exist, leave all player quests as incomplete.
                if (tag.TryGet(TagKey, out string[] quests))
                    LoadCompletedQuests(quests);
            }

            public override void OnEnterWorld()
            {
                foreach (var quest in QuestManager.IncompletePlayerQuests.Select(QuestManager.GetQuest).ToArray())
                {
                    if (quest.CheckCompletion())
                        QuestManager.MarkComplete(quest);
                }
            }
        }

        #endregion

        #region Quest Saving

        public override void SaveWorldData(TagCompound tag)
        {
            if (QuestManager.CompletedWorldQuests is not null)
                tag[TagKey] = QuestManager.CompletedWorldQuests.ToList();
        }

        public partial class PlayerQuestLoader : ModPlayer
        {
            public override void SaveData(TagCompound tag)
            {
                if (QuestManager.CompletedPlayerQuests is not null)
                    tag[TagKey] = QuestManager.CompletedPlayerQuests.ToList();
            }
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

        public static void SaveQuestLog(IList<QuestBook> questLog, string filePath)
        {
            string serialized = JsonConvert.SerializeObject(questLog, Formatting.Indented, JsonTypeResolverFix.Settings);
            File.WriteAllText(filePath, serialized);
        }

        public static List<QuestBook> LoadQuestLog(string filePath)
        {
            string serialized = File.ReadAllText(filePath);

            try
            {
                List<QuestBook> book = JsonConvert.DeserializeObject<List<QuestBook>>(serialized, JsonTypeResolverFix.Settings);
                return book;
            }
            catch
            {
                return null;
            }
        }
    }
}

using QuestBooks.QuestLog;
using QuestBooks.Quests;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using Terraria;

namespace QuestBooks.Systems
{
    internal static class QuestManager
    {
        // These are only string values so that the quests don't have a chance to end
        // up being duplicated by user-error. We only want one loaded copy of each quest.
        public static FrozenDictionary<string, Quest> ActiveQuests { get; internal set; }
        public static string[] CompletedQuests { get; internal set; }
        public static string[] IncompleteQuests { get; internal set; }

        public static FrozenDictionary<string, Quest> WorldQuests { get; internal set; }
        public static string[] CompletedWorldQuests { get; internal set; }
        public static string[] IncompleteWorldQuests { get; internal set; }

        public static FrozenDictionary<string, Quest> PlayerQuests { get; internal set; }
        public static string[] CompletedPlayerQuests { get; internal set; }
        public static string[] IncompletePlayerQuests { get; internal set; }

        public static List<QuestBook> QuestBooks { get; internal set; } = [];
        public static Dictionary<string, QuestLogStyle> QuestLogStyles = null;
        public static QuestLogStyle ActiveStyle = null;

        // Reset "completed" quests
        public static void LoadActiveQuests()
        {
            Dictionary<string, Quest> newActiveQuests = [];
            Dictionary<string, Quest> newWorldQuests = [];
            Dictionary<string, Quest> newPlayerQuests = [];

            foreach (var kvp in QuestLoader.QuestNames)
            {
                var quest = (Quest)Activator.CreateInstance(kvp.Key);
                newActiveQuests.Add(kvp.Value, quest);

                if (quest.QuestType == QuestType.World)
                    newWorldQuests.Add(kvp.Value, quest);

                else if (!Main.dedServ)
                    newPlayerQuests.Add(kvp.Value, quest);
            }

            ActiveQuests = newActiveQuests.ToFrozenDictionary();
            IncompleteQuests = [.. ActiveQuests.Keys];
            CompletedQuests = [];

            WorldQuests = newWorldQuests.ToFrozenDictionary();
            IncompleteWorldQuests = [.. WorldQuests.Keys];
            CompletedWorldQuests = [];

            PlayerQuests = newPlayerQuests.ToFrozenDictionary();
            IncompletePlayerQuests = [.. PlayerQuests.Keys];
            CompletedPlayerQuests = [];
        }

        public static void UnloadActiveQuests()
        {
            ActiveQuests = null;
            CompletedQuests = null;
            IncompleteQuests = null;

            WorldQuests = null;
            CompletedWorldQuests = null;
            IncompleteWorldQuests = null;

            PlayerQuests = null;
            CompletedPlayerQuests = null;
            IncompletePlayerQuests = null;
        }

        public static Quest GetQuest(string questName) => ActiveQuests[questName];
        public static Quest GetQuest<TQuest>() where TQuest : Quest => GetQuest(QuestLoader.QuestNames[typeof(TQuest)]);

        public static bool TryGetQuest(string questName, out Quest result) => ActiveQuests.TryGetValue(questName, out result);
        public static bool TryGetQuest<TQuest>(out TQuest result) where TQuest : Quest
        {
            if (QuestLoader.QuestNames.TryGetValue(typeof(TQuest), out var questName) && TryGetQuest(questName, out var questResult))
            {
                result = (TQuest)questResult;
                return true;
            }

            result = default;
            return false;
        }

        public static void CompleteQuest<TQuest>() where TQuest : Quest => CompleteQuest(GetQuest<TQuest>());
        public static void CompleteQuest(string questName) => CompleteQuest(GetQuest(questName));
        public static void CompleteQuest(Quest quest)
        {
            quest.OnCompletion();
            MarkComplete(quest);
        }

        public static void MarkComplete<TQuest>() where TQuest : Quest => MarkComplete(GetQuest<TQuest>());
        public static void MarkComplete(string questName) => MarkComplete(GetQuest(questName));
        public static void MarkComplete(Quest quest)
        {
            var newIncomplete = new List<string>(IncompleteQuests);
            var newComplete = new List<string>(CompletedQuests);

            newIncomplete.Remove(quest.Key);
            newComplete.Add(quest.Key);

            IncompleteQuests = [.. newIncomplete];
            CompletedQuests = [.. newComplete];

            if (quest.QuestType == QuestType.World)
            {
                var newWorldIncomplete = new List<string>(IncompleteWorldQuests);
                var newWorldComplete = new List<string>(CompletedWorldQuests);

                newWorldIncomplete.Remove(quest.Key);
                newWorldComplete.Add(quest.Key);

                IncompleteWorldQuests = [.. newWorldIncomplete];
                CompletedWorldQuests = [.. newWorldComplete];
            }

            else
            {
                var newPlayerIncomplete = new List<string>(IncompletePlayerQuests);
                var newPlayerComplete = new List<string>(CompletedPlayerQuests);

                newPlayerIncomplete.Remove(quest.Key);
                newPlayerComplete.Add(quest.Key);

                IncompletePlayerQuests = [.. newPlayerIncomplete];
                CompletedPlayerQuests = [.. newPlayerComplete];
            }

            quest.Completed = true;
            quest.MarkAsComplete();
        }
    }
}

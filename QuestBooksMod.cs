global using static QuestBooks.Utilities.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.QuestLog;
using QuestBooks.QuestLog.DefaultStyles;
using QuestBooks.Quests;
using QuestBooks.Systems;
using QuestBooks.Systems.NetCode;
using QuestBooks.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;

namespace QuestBooks
{
    public class QuestBooksMod : Mod
    {
        public static bool DesignerEnabled { get; internal set; } = false;
        public static Mod DesignerMod { get; private set; } = null;

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            var packetType = PacketManager.IdToPacket[reader.ReadByte()];
            var packet = (QuestPacket)Activator.CreateInstance(packetType)!;
            packet.HandlePacket(in reader, whoAmI);
        }

        public override void PostSetupContent()
        {
            EnableDesigner(this);

            foreach (Mod mod in ModLoader.Mods)
                QuestLoader.LoadQuests(mod);

            VanillaQuestBooks.AddVanillaQuests(this);
        }

        #region API

        /// <summary>
        /// Enables the use of the quest book designer in game.<br/>
        /// You should call this inside of <see cref="ModSystem.PostSetupContent"/>.
        /// </summary>
        public static void EnableDesigner(Mod enablingMod)
        {
            DesignerEnabled = true;
            DesignerMod = enablingMod;
        }

        /// <summary>
        /// Deserializes a custom quest log and adds it to the UI.<br/>
        /// You should call this inside of <see cref="ModSystem.PostSetupContent"/>.
        /// </summary>
        public static void AddQuestLog(string questLogKey, string serializedQuestLog, Mod mod)
        {
            var questLog = JsonConvert.DeserializeObject<List<QuestBook>>(serializedQuestLog, JsonTypeResolverFix.Settings);
            AddQuestLog(questLogKey, questLog, mod);
        }

        /// <summary>
        /// Adds a custom quest log to the UI.<br/>
        /// You should call this inside of <see cref="ModSystem.PostSetupContent"/>.
        /// </summary>
        public static void AddQuestLog(string questLogKey, IList<QuestBook> questLog, Mod mod)
        {
            QuestManager.QuestLogs.Add(questLogKey, questLog);
            QuestManager.QuestLogMods.Add(questLogKey, mod);

            QuestLogDrawer.CoverDrawCalls.Add(questLogKey, BasicQuestLogStyle.DrawDefaultCover);
            QuestLogDrawer.LogTitleRetrievalCalls.Add(questLogKey, BasicQuestLogStyle.RetrieveDefaultLogTitle);
            QuestLogDrawer.LogTitleDrawCalls.Add(questLogKey, BasicQuestLogStyle.DrawDefaultLogTitle);
        }

        /// <summary>
        /// Represents a draw delegate for the icon on the cover of the default quest log style implementation.
        /// </summary>
        public delegate void CoverDrawDelegate(SpriteBatch spriteBatch, Vector2 drawCenter, float scale, float rotation);

        /// <summary>
        /// Allows you to modify the drawing logic for the icon on the cover of the book in the default quest log style.<br/>
        /// <br/>
        /// You should call this inside of <see cref="ModSystem.PostSetupContent"/>, <b>AFTER</b> adding your quest log.
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown when attempting to register a draw delegate for a quest log that has not been registered.</exception>
        public static void RegisterCoverDrawDelegate(string questLogKey, CoverDrawDelegate coverDrawDelegate)
        {
            if (!QuestManager.QuestLogs.ContainsKey(questLogKey))
                throw new KeyNotFoundException($"Quest log with key {questLogKey} has not been registered!");

            QuestLogDrawer.CoverDrawCalls[questLogKey] = coverDrawDelegate;
        }

        /// <summary>
        /// Represents a delegate used to retrieve the title for a given quest log.
        /// </summary>
        public delegate string LogTitleRetrievalDelegate(string questLogKey);

        /// <summary>
        /// Allows you to modify how the title of the given quest log is retrieved.<br/>
        /// <br/>
        /// You should call this inside of <see cref="ModSystem.PostSetupContent"/>, <b>AFTER</b> adding your quest log.
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown when attempting to register a draw delegate for a quest log that has not been registered.</exception>
        public static void RegisterLogTitle(string questLogKey, LogTitleRetrievalDelegate logTitleRetrievalDelegate)
        {
            if (!QuestManager.QuestLogs.ContainsKey(questLogKey))
                throw new KeyNotFoundException($"Quest log with key {questLogKey} has not been registered!");

            QuestLogDrawer.LogTitleRetrievalCalls[questLogKey] = logTitleRetrievalDelegate;
        }

        /// <summary>
        /// Represents a draw delegate for the title of a quest log when the user is selecting which quest log to interact with.
        /// </summary>
        public delegate void LogTitleDrawDelegate(SpriteBatch spriteBatch, Rectangle drawArea, string title, float opacity, bool hovered, bool selected);

        /// <summary>
        /// Allows you to modify the drawing logic for the name of your quest log when the user is choosing which log to select.<br/>
        /// <br/>
        /// You should call this inside of <see cref="ModSystem.PostSetupContent"/>, <b>AFTER</b> adding your quest log.
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown when attempting to register a draw delegate for a quest log that has not been registered.</exception>
        public static void RegisterLogTitleDrawDelegate(string questLogKey, LogTitleDrawDelegate logTitleDrawDelegate)
        {
            if (!QuestManager.QuestLogs.ContainsKey(questLogKey))
                throw new KeyNotFoundException($"Quest log with key {questLogKey} has not been registered!");

            QuestLogDrawer.LogTitleDrawCalls[questLogKey] = logTitleDrawDelegate;
        }

        /// <summary>
        /// Disables another quest log (i.e. for replacing the vanilla log).<br/>
        /// You should call this inside of <see cref="ModSystem.PostSetupContent"/>.
        /// </summary>
        public static void DisableQuestLog(string questLogKey)
        {
            if (!QuestManager.DisabledQuestLogs.Contains(questLogKey))
                QuestManager.DisabledQuestLogs.Add(questLogKey);
        }

        /// <summary>
        /// Adds a custom quest log style to be able to used.<br/>
        /// If <paramref name="exclusive"/> is <see langword="true"/>, the passed in style will be the only one able to be used.<br/>
        /// You should call this inside of <see cref="ModSystem.PostSetupContent"/>.
        /// </summary>
        public static void AddQuestLogStyle(QuestLogStyle questLogStyle, Mod mod, bool exclusive = false)
        {
            if (exclusive)
                QuestLoader.ExclusiveOverrideStyle = questLogStyle;

            QuestLoader.LogStyleRegistry.TryAdd(mod, []);
            QuestLoader.LogStyleRegistry[mod].Add(questLogStyle);
        }

        public static Quest GetQuest(string questName) => QuestManager.GetQuest(questName);
        public static Quest GetQuest<TQuest>() where TQuest : Quest => QuestManager.GetQuest<TQuest>();

        public static bool TryGetQuest(string questName, out Quest result) => QuestManager.TryGetQuest(questName, out result);
        public static bool TryGetQuest<TQuest>(out TQuest result) where TQuest : Quest => QuestManager.TryGetQuest(out result);

        public static void CompleteQuest<TQuest>() where TQuest : Quest => QuestManager.CompleteQuest<TQuest>();
        public static void CompleteQuest(string questName) => QuestManager.CompleteQuest(questName);
        public static void CompleteQuest(Quest quest) => QuestManager.CompleteQuest(quest);

        public static void MarkComplete<TQuest>() where TQuest : Quest => QuestManager.MarkComplete<TQuest>();
        public static void MarkComplete(string questName) => QuestManager.MarkComplete(questName);
        public static void MarkComplete(Quest quest) => QuestManager.MarkComplete(quest);

        #endregion
    }
}

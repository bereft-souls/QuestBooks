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
        public static Mod Instance { get; private set; }
        public static bool DesignerEnabled { get; internal set; } = false;
        public static Mod DesignerMod { get; private set; } = null;

        public override void Load()
        {
            Instance = this;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            var packetType = PacketManager.IdToPacket[reader.ReadByte()];
            var packet = (QuestPacket)Activator.CreateInstance(packetType)!;
            packet.HandlePacket(reader, whoAmI);
        }

        public override void PostSetupContent()
        {
            EnableDesigner(this);

            foreach (Mod mod in ModLoader.Mods)
                QuestLoader.LoadQuests(mod);

            VanillaQuestBooks.AddVanillaQuests();
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
        /// You should call this inside of <see cref="ModSystem.PostSetupContent"/>.<br/>
        /// You can supply <paramref name="coverDrawAction"/> to change the icon that draws on the cover of the default quest book.
        /// </summary>
        public static void AddQuestLog(string questLogName, string serializedQuestLog, Action<SpriteBatch, Vector2, float> coverDrawAction = null)
        {
            var questBook = JsonConvert.DeserializeObject<List<QuestBook>>(serializedQuestLog, JsonTypeResolverFix.Settings);
            AddQuestLog(questLogName, questBook, coverDrawAction);
        }

        /// <summary>
        /// Adds a custom quest log to the UI.<br/>
        /// You should call this inside of <see cref="ModSystem.PostSetupContent"/>.<br/>
        /// You can supply <paramref name="coverDrawAction"/> to change the icon that draws on the cover of the default quest book.
        /// </summary>
        public static void AddQuestLog(string questLogName, IList<QuestBook> questLog, Action<SpriteBatch, Vector2, float> coverDrawAction = null)
        {
            QuestManager.QuestLogs.Add(questLogName, questLog);
            QuestLogDrawer.CoverDrawCalls.Add(questLogName, coverDrawAction ?? BasicQuestLogStyle.DrawDefaultCover);
        }

        /// <summary>
        /// Disabled another quest log (i.e. for replacing the vanilla log).<br/>
        /// You should call this inside of <see cref="ModSystem.PostSetupContent"/>.
        /// </summary>
        /// <param name="questLogName"></param>
        public static void DisableQuestLog(string questLogName)
        {
            QuestManager.DisabledQuestLogs.Add(questLogName);
        }

        /// <summary>
        /// Adds a custom quest log style to be able to used.<br/>
        /// If <paramref name="exclusive"/> is <see langword="true"/>, the passed in style will be the only one able to be used.<br/>
        /// You should call this inside of <see cref="ModSystem.PostSetupContent"/>.
        /// </summary>
        public static void AddQuestLogStyle(QuestLogStyle questLog, Mod mod, bool exclusive = false)
        {
            if (exclusive)
                QuestLoader.ExclusiveOverrideStyle = questLog;

            QuestLoader.LogStyleRegistry.TryAdd(mod, []);
            QuestLoader.LogStyleRegistry[mod].Add(questLog);
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

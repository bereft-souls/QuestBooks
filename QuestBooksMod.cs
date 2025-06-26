global using static QuestBooks.Utilities.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using QuestBooks.QuestLog;
using QuestBooks.QuestLog.DefaultQuestBooks;
using QuestBooks.QuestLog.DefaultQuestLogStyles;
using QuestBooks.Quests;
using QuestBooks.Systems;
using QuestBooks.Systems.NetCode;
using QuestBooks.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Terraria.ModLoader;

namespace QuestBooks
{
	public class QuestBooksMod : Mod
	{
        public static Mod Instance { get; private set; }
        public static bool DesignerEnabled { get; set; } = false;

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
            EnableDesigner();
            VanillaQuestBooks.AddVanillaQuests();
            AddQuestLogStyle(new BasicQuestLogStyle(), this);
        }

        #region API

        /// <summary>
        /// Enables the user of the quest book designer in game.<br/>
        /// You should call this inside of <see cref="ModSystem.PostSetupContent"/>.
        /// </summary>
        public static void EnableDesigner()
        {
            DesignerEnabled = true;
        }

        /// <summary>
        /// Deserializes a custom quest book and adds it to the log.<br/>
        /// You should call this inside of <see cref="ModSystem.PostSetupContent"/>.
        /// </summary>
        public static void AddQuestBook(string serializedQuestBook, Mod mod)
        {
            var questBook = JsonConvert.DeserializeObject<QuestBook>(serializedQuestBook, JsonTypeResolverFix.Settings);
            AddQuestBook((QuestBook)questBook, mod);
        }

        /// <summary>
        /// Adds a custom quest book to the log.<br/>
        /// You should call this inside of <see cref="ModSystem.PostSetupContent"/>.
        /// </summary>
        public static void AddQuestBook(QuestBook questBook, Mod mod)
        {
            QuestLoader.LoadQuests(mod);
            QuestManager.QuestBooks.Add(questBook);
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

        // Allows adding a questbook without hard-referencing the assembly.
        // In order to create quests, the assembly still needs to be referenced,
        // but all quests are tagged with ExtendsFromMod by default, so
        // this saves some effort on the front of external programmers.
        public override object Call(params object[] args)
        {
            switch (((string)args[0]).ToLower())
            {
                case "enabledesigner":
                    EnableDesigner();
                    return true;

                case "addquestbook":

                    if (args[2] is not Mod)
                        return new ArgumentException("Invalid arguments: Third argument must be the mod instance that is adding this questbook");

                    switch (args[1])
                    {
                        case QuestBook questBook:
                            AddQuestBook(questBook, (Mod)args[2]);
                            break;
                        case string serialized:
                            AddQuestBook(serialized, (Mod)args[2]);
                            break;
                        default:
                            return new ArgumentException("Invalid arguments: Second argument must either be a QuestBook object, or a serialized QuestBook as outputted by the designer");
                    }

                    return true;

                case "addquestlogstyle":
                    if (args[2] is not Mod)
                        return new ArgumentException("Invalid arguments: Third argument must be the mod instance that is adding this quest log style");

                    bool styleExclusive = args.Length < 4 || args[3] is not bool exclusive || !exclusive;

                    if (args[1] is QuestLogStyle logStyle)
                        AddQuestLogStyle(logStyle, (Mod)args[2], styleExclusive);

                    else
                        return new ArgumentException("Invalid arguments: Second argument must be a quest log style implementation");

                    return true;
            }

            return new ArgumentException("No matching ModCall was found", (string)args[0]);
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

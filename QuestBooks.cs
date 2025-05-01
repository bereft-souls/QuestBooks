using Newtonsoft.Json;
using QuestBooks.QuestLog;
using QuestBooks.QuestLog.DefaultQuestBooks;
using QuestBooks.Quests;
using QuestBooks.Systems;
using QuestBooks.Systems.NetCode;
using System;
using System.IO;
using System.Linq;
using Terraria.ModLoader;

namespace QuestBooks
{
	public class QuestBooks : Mod
	{
        public static Mod Instance { get; private set; }
        public static bool DesignerEnabled { get; private set; } = false;

        public override void Load() => Instance = this;

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            Type packetType = PacketManager.IdToPacket[reader.ReadByte()];
            var packet = (QuestPacket)Activator.CreateInstance(packetType);
            packet.HandlePacket(reader, whoAmI);
        }

        public override void PostSetupContent()
        {
            if (ModLoader.Mods.Any(m => Attribute.GetCustomAttribute(m.GetType(), typeof(EnableDesignerAttribute)) is not null))
                DesignerEnabled = true;
        }

        #region API

        [AttributeUsage(AttributeTargets.Class)]
        public class EnableDesignerAttribute : Attribute
        { }

        // Adds a questbook to the active list.
        public static void AddQuestBook(QuestBook questBook)
        {
            QuestLoader.LoadQuests(questBook);
            QuestManager.QuestBooks.Add(questBook);
        }

        // Allows adding a questbook without hard-referencing the assembly.
        // In order to create quests, the assembly still needs to be referenced,
        // but all quests are tagged with JITWhenModsEnabled by default, so
        // this saves some effort on the front of external programmers.
        public override object Call(params object[] args)
        {
            switch (((string)args[0]).ToLower())
            {
                case "addquestbook":

                    if (args[1] is QuestBook questBook)
                        AddQuestBook(questBook);

                    else if (args[1] is string serialized)
                        AddQuestBook(JsonConvert.DeserializeObject<BasicQuestBook>(serialized));

                    else
                        return new ArgumentException("Invalid arguments: Second argument must either be a QuestBook object, or a serialized QuestBook as outputted by the designer");

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

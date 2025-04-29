using QuestBooks.QuestLog;
using QuestBooks.Quests;
using QuestBooks.Systems;
using QuestBooks.Systems.NetCode;
using System;
using System.Collections.Frozen;
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

        public static void AddQuestBook(QuestBook questBook)
        {
            QuestLoader.LoadQuests(questBook);
            QuestManager.QuestBooks = QuestManager.QuestBooks.Append(questBook).ToFrozenSet();
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

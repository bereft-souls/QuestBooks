using QuestBooks.Content;
using QuestBooks.Systems;
using QuestBooks.Systems.NetCode;
using System;
using System.IO;
using Terraria.ModLoader;

namespace QuestBooks
{
	public class QuestBooks : Mod
	{
        public static Mod Instance { get; private set; }

        public override void Load() => Instance = this;
        public override void Unload() => Instance = null;

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            Type packetType = PacketManager.IdToPacket[reader.ReadByte()];
            var packet = (QuestPacket)Activator.CreateInstance(packetType);
            packet.HandlePacket(reader, whoAmI);
        }

        public static void AddQuestBook(QuestBook questBook)
        {
            QuestLoader.LoadQuests(questBook);
        }
    }
}

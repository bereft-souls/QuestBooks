using System;
using System.IO;
using Terraria.ModLoader;

namespace QuestBooks.Systems.NetCode
{
    internal abstract class QuestPacket
    {
        public virtual void WritePacket(ModPacket modPacket) { }

        public abstract void HandlePacket(in BinaryReader packet, int sender);

        public ModPacket Create()
        {
            var packet = QuestBooksMod.Instance.GetPacket();
            packet.Write(PacketManager.PacketToId[GetType()]);
            return packet;
        }

        public static void Send<TPacket>(Action<ModPacket> extraWriting = null, int toClient = -1, int ignoreClient = -1)
            where TPacket : QuestPacket
        {
            TPacket packet = Activator.CreateInstance<TPacket>();
            var modPacket = packet.Create(); // This pre-writes the packet type ID

            packet.WritePacket(modPacket);
            extraWriting?.Invoke(modPacket);

            modPacket.Send(toClient, ignoreClient);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace QuestBooks.Systems.NetCode
{
    internal class PacketManager : ModSystem
    {
        public static byte PacketTypeCount = 0;

        public static Dictionary<Type, byte> PacketToId = [];
        public static Dictionary<byte, Type> IdToPacket = [];

        public override void Load()
        {
            var types = GetType().Assembly.GetTypes().Where(
                t => !t.IsAbstract && t.IsSubclassOf(typeof(QuestPacket)));

            foreach (var type in types)
            {
                PacketToId.Add(type, PacketTypeCount);
                IdToPacket.Add(PacketTypeCount, type);
                PacketTypeCount++;
            }
        }
    }
}

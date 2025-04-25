using MonoMod.Utils;
using QuestBooks.Systems.NetCode;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Systems
{
    // If the player enters a multiplayer world, they need to send
    // a request to the server to sync up completed world quests.
    internal class QuestSyncPlayer : ModPlayer
    {
        public override void OnEnterWorld()
        {
            if (Player.whoAmI != Main.myPlayer || Main.netMode != NetmodeID.MultiplayerClient)
                return;

            QuestPacket.Send<QuestSyncRequestPacket>();
        }
    }

    // When the server receives a sync request packet,
    // all it needs to do is send the response packet.
    internal class QuestSyncRequestPacket : QuestPacket
    {
        public override void HandlePacket(in BinaryReader packet, int sender) => Send<QuestSyncResponsePacket>(toClient: sender);
    }

    // The response packet contains all the completed world quests.
    internal class QuestSyncResponsePacket : QuestPacket
    {
        public override void WritePacket(ModPacket modPacket)
        {
            // Write the number of completed quests.
            modPacket.Write(QuestManager.CompletedQuests.Length);

            // Write each completed quest.
            foreach (var quest in QuestManager.CompletedQuests)
                modPacket.WriteNullTerminatedString(quest);
        }

        public override void HandlePacket(in BinaryReader packet, int sender)
        {
            // Read the number of completed quests.
            var questCount = packet.ReadInt32();
            List<string> completedQuests = [];

            // Read each completed quest.
            for (int i = 0; i < questCount; i++)
                completedQuests.Add(packet.ReadNullTerminatedString());

            // Mark each completed quest as completed.
            QuestLoader.LoadCompletedQuests(completedQuests);
        }
    }

    // This is received on multiplayer clients when a world quest is completed.
    internal class QuestCompletionPacket : QuestPacket
    {
        public override void HandlePacket(in BinaryReader packet, int sender)
        {
            string questName = packet.ReadNullTerminatedString();
            QuestManager.CompleteQuest(questName);
        }
    }
}

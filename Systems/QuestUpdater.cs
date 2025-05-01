using MonoMod.Utils;
using QuestBooks.Systems.NetCode;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Systems
{
    internal class QuestUpdater : ModSystem
    {
        // World quests are updated in singleplayer and on the server.
        // Not on multiplayer clients.
        public static void UpdateWorldQuests()
        {
            // Save the original array copy so that even if collection modification
            // occurs, enumeration can still continue.
            var incompleteQuests = QuestManager.IncompleteWorldQuests;

            foreach (var questName in incompleteQuests)
            {
                var quest = QuestManager.GetQuest(questName);

                if (quest.CheckCompletion())
                {
                    QuestManager.CompleteQuest(quest);

                    // If this is running on the server, send a packet to each client
                    // containing the quest that was just completed.
                    if (Main.dedServ)
                        QuestPacket.Send<QuestCompletionPacket>((packet) => packet.WriteNullTerminatedString(questName));
                }
            }
        }

        // Player quests are updated in singleplayer and on multiplayer clients.
        // Not on the server.
        public static void UpdatePlayerQuests()
        {
            // Save the original array copy so that even if collection modification
            // occurs, enumeration can still continue.
            var incompleteQuests = QuestManager.IncompletePlayerQuests;

            foreach (var questName in incompleteQuests)
            {
                var quest = QuestManager.GetQuest(questName);

                if (quest.CheckCompletion())
                    QuestManager.CompleteQuest(quest);
            }
        }

        // Loop through and check quest completion post-update.
        public override void PostUpdateEverything()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                UpdateWorldQuests();

            if (!Main.dedServ)
                UpdatePlayerQuests();
        }
    }
}

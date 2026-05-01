using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Systems
{
    internal class QuestUpdater : ModSystem
    {
        // Update the active quest log style.
        public override void UpdateUI(GameTime gameTime)
        {
            foreach (var questBook in QuestManager.QuestBooks)
                questBook.Update();

            QuestLogDrawer.ActiveStyle.UpdateLog();
        }

        // Loop through and check quest completion post-update.
        public override void PostUpdateEverything()
        {
            var allQuests = QuestManager.ActiveQuests.Values.ToArray();

            foreach (var quest in allQuests)
                quest.Update();

            // World quests are updated in singleplayer and on the server.
            // Not on multiplayer clients.
            if (Main.netMode != NetmodeID.MultiplayerClient)
                UpdateIncompleteQuests(QuestManager.IncompleteWorldQuests);

            // Player quests are updated in singleplayer and on multiplayer clients.
            // Not on the server.
            if (!Main.dedServ)
                UpdateIncompleteQuests(QuestManager.IncompletePlayerQuests);
        }

        public static void UpdateIncompleteQuests(string[] incompleteQuests)
        {
            foreach (var questName in incompleteQuests)
            {
                var quest = QuestManager.GetQuest(questName);

                if (quest.CheckCompletion())
                    QuestManager.CompleteQuest(quest);
            }
        }
    }
}

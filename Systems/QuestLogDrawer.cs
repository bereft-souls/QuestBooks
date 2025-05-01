using Microsoft.Xna.Framework;
using QuestBooks.QuestLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace QuestBooks.Systems
{
    internal class QuestLogDrawer : ModSystem
    {
        public override void Load()
        {
            // Prepare render targets.
            Main.OnPreDraw += (gameTime) =>
            {

            };
        }

        public static void DrawQuestLog()
        {

        }

        public override void UpdateUI(GameTime gameTime)
        {
            foreach (QuestBook questBook in QuestManager.QuestBooks)
                questBook.Update();
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (mouseTextLayer == -1)
                return;

            layers.Insert(mouseTextLayer, new LegacyGameInterfaceLayer(
                "QuestBooks: Quest Log", () =>
                {
                    DrawQuestLog();
                    return true;
                }
            ));
        }
    }
}

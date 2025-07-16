using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class EvilBossDefeated : DynamicQuest
    {
        private Asset<Texture2D> iconTexture = null;
        private bool crimsonCache = false;

        public override Asset<Texture2D> Texture
        {
            get
            {
                iconTexture ??= crimsonCache ?
                    ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/Quests/BrainOfCthulhu") :
                    ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/Quests/EaterOfWorlds");

                return iconTexture;
            }
        }

        public override void Update()
        {
            if (WorldGen.crimson != crimsonCache)
            {
                crimsonCache = WorldGen.crimson;
                iconTexture = null;
            }
        }

        public override bool CheckCompletion() => NPC.downedBoss2;
    }
}

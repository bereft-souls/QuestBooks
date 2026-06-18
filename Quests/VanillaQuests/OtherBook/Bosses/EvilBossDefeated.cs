using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using Terraria;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class EvilBossDefeated : QBDynamicQuest
    {
        public static bool Crimson => WorldGen.crimson;
        public override string Name => Crimson ? "BrainofCthulhu" : "EaterofWorlds";
        public override bool CheckCompletion() => NPC.downedBoss2;

        private readonly Texture2D brain = Main.dedServ ? null : ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/Quests/Book0/Chapter1/BrainOfCthulhu").Value;
        private readonly Texture2D worm = Main.dedServ ? null : ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/Quests/Book0/Chapter1/EaterOfWorlds").Value;

        public Texture2D OutlineTexture { get; } = Main.dedServ ? null : ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/Quests/LargeOutline").Value;
        public Texture2D IconTexture => Crimson ? brain : worm;

        public override void DrawLocked(SpriteBatch spriteBatch, Vector2 canvasPosition, Vector2 canvasOffset, float zoom, bool hovered, bool selected)
        {
            DrawOutline(spriteBatch, canvasPosition, canvasOffset, zoom, Color.Black);
            DrawTexture(spriteBatch, IconTexture, canvasPosition, canvasOffset, zoom, Color.Black);
        }

        public override void DrawIncomplete(SpriteBatch spriteBatch, Vector2 canvasPosition, Vector2 canvasOffset, float zoom, bool hovered, bool selected)
        {
            if (selected)
                DrawOutline(spriteBatch, canvasPosition, canvasOffset, zoom, Color.Yellow);

            else if (hovered)
                DrawOutline(spriteBatch, canvasPosition, canvasOffset, zoom, Color.LightGray);

            else
                DrawOutline(spriteBatch, canvasPosition, canvasOffset, zoom, Color.Gray);

            Effect grayscale = QuestAssets.Grayscale;
            spriteBatch.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, grayscale, matrix);
            DrawTexture(spriteBatch, IconTexture, canvasPosition, canvasOffset, zoom, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
        }

        public override void DrawCompleted(SpriteBatch spriteBatch, Vector2 canvasPosition, Vector2 canvasOffset, float zoom, bool hovered, bool selected)
        {
            if (selected)
                DrawOutline(spriteBatch, canvasPosition, canvasOffset, zoom, Color.Yellow);

            else if (hovered)
                DrawOutline(spriteBatch, canvasPosition, canvasOffset, zoom, Color.LightGray);

            else
                DrawOutline(spriteBatch, canvasPosition, canvasOffset, zoom, new(108, 118, 199, 255));

            DrawTexture(spriteBatch, IconTexture, canvasPosition, canvasOffset, zoom, Color.White);
        }

        protected void DrawOutline(SpriteBatch spriteBatch, Vector2 canvasPosition, Vector2 canvasOffset, float zoom, Color color) =>
            DrawTexture(spriteBatch, OutlineTexture, canvasPosition, canvasOffset, zoom, color);

        protected static void DrawTexture(SpriteBatch spriteBatch, Texture2D texture, Vector2 canvasPosition, Vector2 canvasOffset, float zoom, Color color)
        {
            Vector2 drawPos = (canvasPosition - canvasOffset) * zoom;
            spriteBatch.Draw(texture, drawPos, null, color, 0f, texture.Size() * 0.5f, zoom, SpriteEffects.None, 0f);
        }

        public override Vector2 HoverAreaSize(bool unlocked) => IconTexture.Size();
    }
}

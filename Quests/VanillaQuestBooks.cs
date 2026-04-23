using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultStyles;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace QuestBooks.Quests
{
    public static class VanillaQuestBooks
    {
        public static void AddVanillaQuests(Mod mod)
        {
            QuestBooksMod.AddQuestLogStyle(new BasicQuestLogStyle(), mod);

            var questLogBytes = mod.GetFileBytes("Quests/VanillaQuestLog.json");
            var questLogString = Encoding.UTF8.GetString(questLogBytes);

            QuestBooksMod.AddQuestLog("Terraria", questLogString, mod);
            QuestBooksMod.RegisterLogTitleDrawDelegate("Terraria", DrawTerrariaLogo);
        }

        public static void DrawTerrariaLogo(SpriteBatch spriteBatch, Rectangle drawArea, string title, float opacity, bool hovered, bool selected)
        {
            Texture2D logo = QuestAssets.TerrariaLogo;
            Texture2D outline = QuestAssets.TerrariaLogoOutline;

            float scale = float.Min(drawArea.Width / (float)logo.Width, drawArea.Height / (float)logo.Height);
            Vector2 drawPos = drawArea.Center();
            Vector2 origin = logo.Size() * 0.5f;

            if (hovered)
                spriteBatch.Draw(outline, drawPos, null, Color.White * opacity, 0f, origin, scale, SpriteEffects.None, 0f);

            spriteBatch.Draw(logo, drawPos, null, Color.White * opacity, 0f, origin, scale, SpriteEffects.None, 0f);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace QuestBooks.QuestLog
{
    [ExtendsFromMod("QuestBooks")]
    public abstract class QuestLogStyle
    {
        public abstract string Key { get; }
        public abstract string DisplayName { get; }
        public virtual bool UseDesigner { get; set; } = false;

        public virtual QuestBook SelectedBook { get; set; } = null;
        public virtual QuestChapter SelectedChapter { get; set; } = null;

        public virtual QuestLogElement SelectedElement { get; set; } = null;
        public virtual QuestLogElement HoveredElement { get; set; } = null;
        public virtual Vector2 QuestAreaOffset { get; set; } = Vector2.Zero;
        public virtual float Zoom { get; set; } = 1f;

        public virtual void OnSelect() { }
        public virtual void OnDeselect() { }

        public virtual void OnEnterWorld() { }
        public virtual void OnToggle(bool active) { }

        public virtual void UpdateLog() { }
        public abstract void DrawLog(SpriteBatch spriteBatch);
        public virtual void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) { }

        public delegate void IconDrawDelegate(SpriteBatch spriteBatch, Texture2D texture, Vector2 center, float scale, bool hovered);

        public virtual void DrawQuestLogIcon(SpriteBatch spriteBatch, Rectangle iconArea, bool hovered)
        {
            spriteBatch.End();
            spriteBatch.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
            spriteBatch.Begin(SpriteSortMode.Deferred, blend, SamplerState.PointClamp, depth, raster, effect, matrix);

            Vector2 center = iconArea.Center();
            float hoverScale = hovered ? 1.1f : 1f;
            float scale = (float)iconArea.Width / QuestAssets.QuestBookIcon.Asset.Width * hoverScale;

            IconDrawDelegate outlineDrawAction = DefaultDrawOutline;
            IconDrawDelegate iconDrawAction = DefaultDrawIcon;

            float outlineDrawPriority = 0f;
            float iconDrawPriority = 0f;

            foreach (QuestBook questBook in QuestManager.QuestBooks)
            {
                questBook.OverrideIconOutlineDraw(ref outlineDrawPriority, ref outlineDrawAction);
                questBook.OverrideIconDraw(ref iconDrawPriority, ref iconDrawAction);
            }

            outlineDrawAction(spriteBatch, QuestAssets.QuestBookOutline, center, scale, hovered);
            iconDrawAction(spriteBatch, QuestAssets.QuestBookIcon, center, scale, hovered);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
        }

        private static void DefaultDrawOutline(SpriteBatch spriteBatch, Texture2D texture, Vector2 center, float scale, bool hovered)
        {
            Color outlineColor = hovered ? Color.Lerp(Color.Yellow, Color.White, 0.2f) : Color.LightYellow;
            spriteBatch.Draw(texture, center, null, outlineColor, 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

        private static void DefaultDrawIcon(SpriteBatch spriteBatch, Texture2D texture, Vector2 center, float scale, bool hovered)
        {
            spriteBatch.Draw(texture, center, null, Color.White, 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

        public abstract void SelectBook(QuestBook book);
        public abstract void SelectChapter(QuestChapter chapter);

        public virtual void SavePlayerData(TagCompound tag) { }
        public virtual void SaveWorldData(TagCompound tag) { }
        public virtual void LoadPlayerData(TagCompound tag) { }
        public virtual void LoadWorldData(TagCompound tag) { }
    }
}

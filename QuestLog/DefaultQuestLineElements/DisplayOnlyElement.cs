using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using ReLogic.Content;
using System;
using Terraria.ModLoader;

namespace QuestBooks.QuestLog.DefaultQuestLineElements
{
    /// <summary>
    /// A simple display element that moves with the canvas.
    /// </summary>
    public class DisplayOnlyElement : QuestLineElement
    {
        public Vector2 CanvasPosition { get; set; }

        public string Texture { get; set; }

        [JsonIgnore]
        public Texture2D DrawTexture { get => ModContent.Request<Texture2D>(Texture, AssetRequestMode.AsyncLoad).Value; }

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset)
        {
            throw new NotImplementedException();
        }
    }
}

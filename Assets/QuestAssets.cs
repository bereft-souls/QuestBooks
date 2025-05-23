using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QuestBooks.Assets
{
    public class QuestAssets : ModSystem
    {
        public static LazyTexture MagicPixel { get; } = new("Terraria/Images/MagicPixel", true);

        public static LazyTexture BasicQuestCanvas { get; } = new("QuestLogCanvas");
        public static LazyTexture ResizeIndicator { get; } = new("ResizeIndicator");

        public override void PostSetupContent()
        {
            // Force an early load of lazy assets.
            foreach (var property in typeof(QuestAssets).GetProperties().Where(p => p.PropertyType == typeof(LazyTexture)))
                ((LazyTexture)property.GetValue(null)).Texture.Size();
        }
    }

    public class LazyTexture(string asset, bool fullString = false) : Lazy<Asset<Texture2D>>(() =>
        ModContent.Request<Texture2D>($"{(fullString ? "" : "QuestBooks/Assets/Textures/")}{asset}"))
    {
        public Texture2D Texture => Value.Value;
        public static implicit operator Texture2D(LazyTexture texture) => texture.Texture;
    }
}

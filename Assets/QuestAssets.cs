using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace QuestBooks.Assets
{
    public class QuestAssets : ModSystem
    {
        public static LazyTexture BasicQuestCanvas { get; } = new("QuestLogCanvas");

        public override void PostSetupContent()
        {
            // Force an early load of lazy assets.
            foreach (var property in typeof(QuestAssets).GetProperties().Where(p => p.PropertyType == typeof(LazyTexture)))
                property.GetValue(null);
        }
    }

    public class LazyTexture(string asset) : Lazy<Asset<Texture2D>>(() => ModContent.Request<Texture2D>($"QuestBooks/Assets/{asset}"))
    {
        public Texture2D Texture => Value.Value;
        public static implicit operator Texture2D(LazyTexture texture) => texture.Texture;
    }
}

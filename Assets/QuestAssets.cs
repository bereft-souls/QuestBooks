using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QuestBooks.Assets
{
    public class QuestAssets : ModSystem
    {
        public static LazyTexture MagicPixel { get; } = new("Terraria/Images/MagicPixel", true);

        public static LazyTexture QuestBookIcon { get; } = new("QuestBookIcon");
        public static LazyTexture QuestBookOutline { get; } = new("QuestBookOutline");

        public static LazyTexture BasicQuestCanvas { get; } = new("QuestLogCanvas");
        public static LazyTexture ResizeIndicator { get; } = new("ResizeIndicator");
        public static LazyTexture LogEntryBackground { get; } = new("LogEntryBackground");
        public static LazyTexture LogEntryBorder { get; } = new("LogEntryBorder");

        public static LazyShader FadedEdges { get; } = new("FadedEdges");

        public override void PostSetupContent()
        {
            // Force an early load of lazy assets.
            foreach (var property in typeof(QuestAssets).GetProperties().Where(p => p.PropertyType.IsAssignableTo(typeof(ILazy))))
                ((ILazy)property.GetValue(null)).WaitAction();
        }
    }

    public class LazyTexture(string asset, bool fullString = false) :
        LazyAsset<Texture2D>($"{(fullString ? "" : "QuestBooks/Assets/Textures/")}{asset}")
    { }

    public class LazyShader(string asset) :
        LazyAsset<Effect>($"QuestBooks/Assets/Shaders/{asset}")
    { }

    public class LazyAsset<T>(string asset) : Lazy<Asset<T>>(() => ModContent.Request<T>(asset)), ILazy
        where T : class
    {
        public Asset<T> ContentAsset => Value;
        public T Asset => Value.Value;
        public Action WaitAction => Value.Wait;

        public static implicit operator T(LazyAsset<T> lazyAsset) => lazyAsset.Asset;
    }

    public interface ILazy
    {
        public Action WaitAction { get; }
    }
}

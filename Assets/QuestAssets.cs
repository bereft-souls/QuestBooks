using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace QuestBooks.Assets
{
    /// <summary>
    /// Contains easy access to assets from the QuestBooks mod.
    /// </summary>
    public class QuestAssets : ModSystem
    {
        public static LazyTexture MagicPixel { get; } = new("Terraria/Images/MagicPixel", true);

        public static LazyTexture QuestBookIcon { get; } = new("QuestBookIcon");
        public static LazyTexture QuestBookOutline { get; } = new("QuestBookOutline");

        public static LazyTexture BasicQuestCanvas { get; } = new("QuestLogCanvas");
        public static LazyTexture ResizeIndicator { get; } = new("ResizeIndicator");
        public static LazyTexture LogEntryBackground { get; } = new("LogEntryBackground");
        public static LazyTexture LogEntryBorder { get; } = new("LogEntryBorder");

        public static LazyTexture MissingIcon { get; } = new("QuestionMark");

        public static LazyShader FadedEdges { get; } = new("FadedEdges");

        public override void PostSetupContent()
        {
            // Don't load assets on server.
            if (Main.dedServ)
                return;

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

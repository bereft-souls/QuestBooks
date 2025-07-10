using Microsoft.Xna.Framework;
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
        public static LazyTexture BigPixel { get; } = new("BigPixel");

        #region QuestLog

        public static LazyTexture QuestBookIcon { get; } = new("QuestLog/QuestBookIcon");
        public static LazyTexture QuestBookOutline { get; } = new("QuestLog/QuestBookOutline");

        public static LazyTexture BasicQuestCanvas { get; } = new("QuestLog/QuestLogCanvas");
        public static LazyTexture ResizeIndicator { get; } = new("QuestLog/ResizeIndicator");

        public static LazyTexture BookTab { get; } = new("QuestLog/BookTab");
        public static LazyTexture BookTabBorder { get; } = new("QuestLog/BookTabBorder");
        public static LazyTexture BookTabGradient { get; } = new("QuestLog/BookTabGradient");

        public static LazyTexture BookScroll { get; } = new("QuestLog/BookScroll");
        public static LazyTexture BookScrollBorder { get; } = new("QuestLog/BookScrollBorder");
        public static LazyTexture ChapterScroll { get; } = new("QuestLog/ChapterScroll");
        public static LazyTexture ChapterScrollBorder { get; } = new("QuestLog/ChapterScrollBorder");

        public static LazyShader FadedEdges { get; } = new("FadedEdges");

        #endregion

        #region Elements

        public static LazyTexture MissingIcon { get; } = new("Elements/QuestionMark", immediateLoad: false);
        public static LazyTexture MissingIconOutline { get; } = new("Elements/QuestionMarkOutline", immediateLoad: false);
        public static LazyTexture Connector { get; } = new("Elements/Connector", immediateLoad: false);
        public static LazyTexture ConnectorPoint { get; } = new("Elements/ConnectorPoint");

        #endregion

        #region Designer

        public static LazyTexture ToggleDesigner { get; } = new("Designer/ToggleDesigner", immediateLoad: false);
        public static LazyTexture ToggleDesignerHovered { get; } = new("Designer/ToggleDesignerHovered", immediateLoad: false);

        public static LazyTexture AddButton { get; } = new("Designer/AddButton", immediateLoad: false);
        public static LazyTexture AddButtonHovered { get; } = new("Designer/AddButtonHovered", immediateLoad: false);

        public static LazyTexture DeleteButton { get; } = new("Designer/DeleteButton", immediateLoad: false);
        public static LazyTexture DeleteButtonHovered { get; } = new("Designer/DeleteButtonHovered", immediateLoad: false);

        public static LazyTexture ExportButton { get; } = new("Designer/ExportButton", immediateLoad: false);
        public static LazyTexture ExportButtonHovered { get; } = new("Designer/ExportButtonHovered", immediateLoad: false);

        public static LazyTexture ExportAllButton { get; } = new("Designer/ExportAllButton", immediateLoad: false);
        public static LazyTexture ExportAllButtonHovered { get; } = new("Designer/ExportAllButtonHovered", immediateLoad: false);

        public static LazyTexture ImportButton { get; } = new("Designer/ImportButton", immediateLoad: false);
        public static LazyTexture ImportButtonHovered { get; } = new("Designer/ImportButtonHovered", immediateLoad: false);

        public static LazyTexture ImportAllButton { get; } = new("Designer/ImportAllButton", immediateLoad: false);
        public static LazyTexture ImportAllButtonHovered { get; } = new("Designer/ImportAllButtonHovered", immediateLoad: false);

        public static LazyTexture ShiftingCanvas { get; } = new("Designer/ShiftingCanvas", immediateLoad: false);
        public static LazyTexture ShiftingCanvasHovered { get; } = new("Designer/ShiftingCanvasHovered", immediateLoad: false);
        public static LazyTexture CanvasCorner { get; } = new("Designer/CanvasCorner", immediateLoad: false);
        public static LazyTexture CanvasCenter { get; } = new("Designer/CanvasCenter", immediateLoad: false);

        public static LazyTexture ToggleGrid { get; } = new("Designer/ToggleGrid", immediateLoad: false);
        public static LazyTexture ToggleGridHovered { get; } = new("Designer/ToggleGridHovered", immediateLoad: false);

        public static LazyTexture ToggleBackdropEnabled { get; } = new("Designer/ToggleBackdropEnabled", immediateLoad: false);
        public static LazyTexture ToggleBackdropEnabledHovered { get; } = new("Designer/ToggleBackdropEnabledHovered", immediateLoad: false);

        public static LazyTexture ToggleBackdropDisabled { get; } = new("Designer/ToggleBackdropDisabled", immediateLoad: false);
        public static LazyTexture ToggleBackdropDisabledHovered { get; } = new("Designer/ToggleBackdropDisabledHovered", immediateLoad: false);

        public static LazyTexture GridSnapping { get; } = new("Designer/GridSnapping", immediateLoad: false);
        public static LazyTexture GridSnappingHovered { get; } = new("Designer/GridSnappingHovered", immediateLoad: false);

        public static LazyTexture GridSize { get; } = new("Designer/GridSize", immediateLoad: false);
        public static LazyTexture GridSizeHovered { get; } = new("Designer/GridSizeHovered", immediateLoad: false);

        public static LazyTexture GridSizeUp { get; } = new("Designer/GridSizeUp", immediateLoad: false);
        public static LazyTexture GridSizeUpHovered { get; } = new("Designer/GridSizeUpHovered", immediateLoad: false);

        public static LazyTexture GridSizeDown { get; } = new("Designer/GridSizeDown", immediateLoad: false);
        public static LazyTexture GridSizeDownHovered { get; } = new("Designer/GridSizeDownHovered", immediateLoad: false);

        public static LazyTexture ToolOutline { get; } = new("Designer/ToolOutline", immediateLoad: false);

        #endregion

        public override void PostSetupContent()
        {
            // Don't load assets on server.
            if (Main.dedServ)
                return;

            // Force an early load of lazy assets.
            foreach (var property in typeof(QuestAssets).GetProperties().Where(p => p.PropertyType.IsAssignableTo(typeof(ILazy))))
            {
                var asset = (ILazy)property.GetValue(null);

                if (asset.ImmediateLoad)
                    asset.WaitAction();
            }
        }
    }

    public class PatchRectangle(string asset, int leftWidth, int rightWidth, int topWidth, int bottomWidth, bool repeatEdges) : LazyTexture(asset)
    {
        public PatchRectangle(string asset, Point cornerSize) :
            this(asset, cornerSize.X, cornerSize.X, cornerSize.Y, cornerSize.Y, false)
        { }

        public PatchRectangle(string asset, Point topLeft, Point bottomRight) :
            this(asset, topLeft.X, bottomRight.X, topLeft.Y, bottomRight.Y, false)
        { }

        public PatchRectangle(string asset, Point cornerSize, bool repeatEdges) :
            this(asset, cornerSize.X, cornerSize.X, cornerSize.Y, cornerSize.Y, repeatEdges)
        { }

        public PatchRectangle(string asset, Point topLeft, Point bottomRight, bool repeatEdges) :
            this(asset, topLeft.X, bottomRight.X, topLeft.Y, bottomRight.Y, repeatEdges)
        { }

        public int Left { get; init; } = leftWidth;
        public int Right { get; init; } = rightWidth;
        public int Top { get; init; } = topWidth;
        public int Bottom { get; init; } = bottomWidth;
        public bool RepeatEdges { get; init; } = repeatEdges;
    }

    public class LazyTexture(string asset, bool fullString = false, bool immediateLoad = true) :
        LazyAsset<Texture2D>($"{(fullString ? "" : "QuestBooks/Assets/Textures/")}{asset}", immediateLoad)
    { }

    public class LazyShader(string asset) :
        LazyAsset<Effect>($"QuestBooks/Assets/Shaders/{asset}")
    { }

    public class LazyAsset<T>(string asset, bool immediateLoad = true) : Lazy<Asset<T>>(() => ModContent.Request<T>(asset)), ILazy
        where T : class
    {
        public Asset<T> ContentAsset => Value;
        public T Asset => Value.Value;
        public Action WaitAction => Value.Wait;

        private readonly bool _immediateLoad = immediateLoad;
        public bool ImmediateLoad => _immediateLoad;

        public static implicit operator T(LazyAsset<T> lazyAsset) => lazyAsset.Asset;
    }

    public interface ILazy
    {
        public bool ImmediateLoad { get; }
        public Action WaitAction { get; }
    }
}

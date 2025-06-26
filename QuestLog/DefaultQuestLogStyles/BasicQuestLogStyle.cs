using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultQuestBooks;
using QuestBooks.QuestLog.DefaultQuestLines;
using QuestBooks.Systems;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace QuestBooks.QuestLog.DefaultQuestLogStyles
{
    public partial class BasicQuestLogStyle : QuestLogStyle
    {
        public override string Key => "DefaultQuestLog";
        public override string DisplayName => "Book";

        // Available element retrieval
        public static IEnumerable<BasicQuestBook> AvailableBooks { get => QuestManager.QuestBooks.Where(b => b is BasicQuestBook).Cast<BasicQuestBook>(); }
        public static IEnumerable<BasicQuestLine> AvailableChapters { get => SelectedBook?.Chapters.Where(c => c is BasicQuestLine).Cast<BasicQuestLine>() ?? []; }

        // These are registered on load
        public static readonly List<Type> AvailableQuestBookTypes = [];
        public static readonly List<Type> AvailableQuestLineTypes = [];
        public static readonly Dictionary<Type, QuestLineElement> AvailableQuestElementTypes = [];

        // Mouse position on canvas
        protected static Vector2 ScaledMousePos { get; set; }
        protected static Point MouseCanvas { get; set; }

        // Handles the render targets for the book/chapter/quest areas,
        // as well as the faded edges handled by the shader
        private static BlendState LayerBlending { get; } = new()
        {
            ColorSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.One,
            AlphaDestinationBlend = Blend.One,
            AlphaBlendFunction = BlendFunction.Add,
        };

        private static BlendState LibraryBlending { get; } = new()
        {
            ColorSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.DestinationAlpha,
            AlphaBlendFunction = BlendFunction.Max
        };

        private static BlendState ContentBlending { get; } = new()
        {
            ColorSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.One,
            AlphaBlendFunction = BlendFunction.Add
        };

        private static BlendState GridBlending { get; } = new()
        {
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.One,
            ColorBlendFunction = BlendFunction.Max,
            AlphaSourceBlend = Blend.One,
            AlphaDestinationBlend = Blend.One,
            AlphaBlendFunction = BlendFunction.Max
        };

        private static RenderTargetBinding[] returnTargets = null;
        private static RenderTarget2D booksTarget = null;
        private static RenderTarget2D chaptersTarget = null;
        private static RenderTarget2D questAreaTarget = null;
        private static RenderTarget2D previousQuestAreaTarget = null;
        private static RenderTarget2D questInfoTarget = null;
        private const float FadeDesignation = 0.025f;

        #region Element Referentials

        private static bool wantsRetarget = true;
        private const float scrollAcceleration = 0.2f;

        #endregion

        #region Draw Parameters

        public static Vector2 LogPositionOffset { get; set; } = Vector2.Zero;
        public static float LogScale { get; set; } = 1f;
        public static bool UseDesigner { get; set; } = false;

        private static Rectangle LogArea;
        private static readonly List<Action<SpriteBatch>> DrawTasks = [];

        #endregion

        public override void OnSelect()
        {
            SetupTargets();
            QuestAssets.FadedEdges.Asset.Parameters["FadeDesignation"].SetValue(FadeDesignation);
        }

        public override void OnDeselect()
        {
            booksTarget?.Dispose();
            chaptersTarget?.Dispose();
            questAreaTarget?.Dispose();
            previousQuestAreaTarget?.Dispose();
            questInfoTarget?.Dispose();
            wantsRetarget = true;
        }

        public override void OnToggle(bool active)
        {
            if (active)
                SoundEngine.PlaySound(SoundID.MenuOpen);

            else
                SoundEngine.PlaySound(SoundID.MenuClose);
        }

        public override void UpdateLog()
        {
            UpdateMouseClicks();
            DrawTasks.Clear();

            if (!QuestLogDrawer.DisplayLog)
                return;

            if (wantsRetarget)
            {
                SetupTargets();
                wantsRetarget = false;
            }

            // Scale MouseCanvas around the center of the screen.
            //
            // If Main.UIScale is greater than 1, the quest canvas will bleed past the edges of the screen, and
            // if it is less than 1, the canvas will not fill the edges of the screen.
            //
            // MouseCanvas is the position within the bounds of the canvas itself, even if said canvas does not exactly
            // match the bounds of the screen.
            LogArea = CalculateLogArea(out var logSize, out var halfScreen, out var halfRealScreen);
            UpdateMousePosition(halfScreen, halfRealScreen);

            Texture2D logTexture = QuestAssets.BasicQuestCanvas;
            DrawTasks.Add(sb => sb.Draw(logTexture, halfRealScreen + (LogPositionOffset * halfRealScreen), null, Color.White, 0f, logTexture.Size() * 0.5f, LogScale, SpriteEffects.None, 0f));  

            // These handle moving the canvas via dragging the book's spine,
            // resizing with the tab in the bottom-right hand corner,
            // and toggling between the designer and normal log.
            UpdateCanvasMovement(halfRealScreen, logSize);
            UpdateCanvasResizing(halfRealScreen);
            UpdateDesignerToggle();

            // Calculate the area designation for books, chapters, and quest contents.
            // These are scaled and repositioned based on the log's screen position and scale.
            CalculateLibraryRegions(LogArea, out var books, out var chapters, out var questArea, out var questInfo);

            // Render each region to it's own render target to allow for shaders.
            // We use a shader to fade out the edges of each target when drawing to the log.

            UpdateBooks(booksTarget.Bounds.CreateScaledMargins(top: FadeDesignation, bottom: FadeDesignation), (MouseCanvas - books.Location).ToVector2() * (booksTarget.Bounds.Size() / books.Size()));            
            UpdateChapters(chaptersTarget.Bounds.CreateScaledMargins(top: FadeDesignation, bottom: FadeDesignation), (MouseCanvas - chapters.Location).ToVector2() * (chaptersTarget.Bounds.Size() / chapters.Size()));
            UpdateQuestArea(questAreaTarget.Bounds.CreateScaledMargin(FadeDesignation), (MouseCanvas - questArea.Location).ToVector2() * (questAreaTarget.Bounds.Size() / questArea.Size()));

            // Render all of the completed render targets with fading applied to the
            // desired edges (top/bottom on books/chapters, all edges on quest area)
            DrawTasks.Add(sb =>
            {
                sb.End();
                QuestAssets.FadedEdges.Asset.Parameters["FadeSides"].SetValue(false);
                sb.Begin(SpriteSortMode.Deferred, LayerBlending, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, QuestAssets.FadedEdges);

                float resizeScale = LogScale / targetScale;

                sb.Draw(booksTarget, books.Center(), null, Color.White, 0f, booksTarget.Size() * 0.5f, resizeScale, SpriteEffects.None, 0f);
                sb.Draw(chaptersTarget, chapters.Center(), null, Color.White, 0f, chaptersTarget.Size() * 0.5f, resizeScale, SpriteEffects.None, 0f);

                sb.End();
                QuestAssets.FadedEdges.Asset.Parameters["FadeSides"].SetValue(true);
                sb.Begin(SpriteSortMode.Deferred, LayerBlending, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, QuestAssets.FadedEdges);

                sb.Draw(questAreaTarget, questArea.Center(), null, Color.White, 0f, questAreaTarget.Size() * 0.5f, resizeScale, SpriteEffects.None, 0f);

                sb.End();
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            });

            if (UseDesigner)
                UpdateDesigner(books, chapters, questArea);
        }

        // This allows us to practically handle updating and drawing at the same time,
        // while skipping over performing the actual draw work when it is not necessary.
        public override void DrawLog(SpriteBatch spriteBatch)
        {
            foreach (var drawAction in DrawTasks)
                drawAction(spriteBatch);
        }

        #region Area Calculation

        // Queues a rectangle for drawing.
        private static Rectangle AddRectangle(Rectangle rectangle, Color color, float stroke = 2f, bool fill = false)
        {
            DrawTasks.Add(sb => sb.DrawRectangle(rectangle, color, stroke, fill));
            return rectangle;
        }

        private static Rectangle CalculateLogArea(out Vector2 logSize, out Vector2 halfScreen, out Vector2 halfRealScreen, float? scaleOverride = null)
        {
            halfScreen = Main.ScreenSize.ToVector2() * 0.5f;
            halfRealScreen = QuestLogDrawer.RealScreenSize * 0.5f;
            logSize = QuestAssets.BasicQuestCanvas.Asset.Size() * (scaleOverride ?? LogScale);
            return CenteredRectangle(halfRealScreen + (LogPositionOffset * halfRealScreen), logSize);
        }

        private static void CalculateLibraryRegions(Rectangle logArea, out Rectangle books, out Rectangle chapters, out Rectangle questArea, out Rectangle questInfo)
        {
            questArea = logArea.CookieCutter(new(0.485f, -0.055f), new(0.45f, 0.84f));
            questInfo = logArea.CookieCutter(new(-0.5025f, -0.055f), new(0.43f, 0.84f)); // The combined area of the Books and Chapters

            // Distance between books/chapters is NOT scaled
            books = questInfo.CookieCutter(new(-0.5f, 0f), new(0.5f, 1f)).CreateMargins(right: 4);
            chapters = questInfo.CookieCutter(new(0.5f, 0f), new(0.5f, 1f)).CreateMargins(left: 4);
        }

        #endregion

        #region Render Targets

        private static void SwitchTargets(RenderTarget2D renderTarget, BlendState blendState = null)
        {
            blendState ??= LayerBlending;

            if (renderTarget is null)
            {
                DrawTasks.Add(sb =>
                {
                    sb.End();
                    sb.GraphicsDevice.SetRenderTargets(returnTargets);
                    returnTargets = null;
                    sb.Begin(SpriteSortMode.Deferred, blendState);
                });

                return;
            }

            DrawTasks.Add(sb =>
            {
                sb.End();
                returnTargets = sb.GraphicsDevice.GetRenderTargets();
                sb.GraphicsDevice.SetRenderTarget(renderTarget);
                sb.Begin(SpriteSortMode.Deferred, blendState, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
            });
        }

        private static void SetupTargets()
        {
            QuestAssets.BasicQuestCanvas.Value.Wait();
            var basicLogArea = CalculateLogArea(out _, out _, out _, LogScale);
            targetScale = LogScale;
            CalculateLibraryRegions(basicLogArea, out Rectangle books, out Rectangle chapters, out Rectangle questArea, out Rectangle questInfo);

            books.Width = books.Width.ToNearestDoubleEven();
            books.Height = books.Height.ToNearestDoubleEven();

            chapters.Width = chapters.Width.ToNearestDoubleEven();
            chapters.Height = chapters.Height.ToNearestDoubleEven();

            questArea.Width = questArea.Width.ToNearestDoubleEven();
            questArea.Height = questArea.Height.ToNearestDoubleEven();

            questInfo.Width = questInfo.Width.ToNearestDoubleEven();
            questInfo.Height = questInfo.Height.ToNearestDoubleEven();

            void TargetSetup()
            {
                static RenderTarget2D GenerateTarget(int width, int height)
                {
                    return new(Main.graphics.GraphicsDevice,
                        width,
                        height,
                        false,
                        SurfaceFormat.Color,
                        DepthFormat.None,
                        0,
                        RenderTargetUsage.PreserveContents);
                }

                var oldBooks = booksTarget;
                var oldChapters = chaptersTarget;
                var oldQuests = questAreaTarget;
                var oldInfo = questInfoTarget;
                var oldPreviousQuests = previousQuestAreaTarget;

                booksTarget = GenerateTarget(books.Width, books.Height);
                chaptersTarget = GenerateTarget(chapters.Width, chapters.Height);
                questAreaTarget = GenerateTarget(questArea.Width, questArea.Height);
                questInfoTarget = GenerateTarget(questInfo.Width, questInfo.Height);
                previousQuestAreaTarget = GenerateTarget(questArea.Width, questArea.Height);

                oldBooks?.Dispose();
                oldChapters?.Dispose();
                oldQuests?.Dispose();
                oldInfo?.Dispose();
                oldPreviousQuests?.Dispose();
            }

            if (ThreadCheck.IsMainThread)
                TargetSetup();

            else
                Main.RunOnMainThread(TargetSetup);
        }

        #endregion

        #region State Caching

        private const string ScaleKey = "QuestBooksScale";
        private const string OffsetKey = "QuestBooksOffset";

        public override void SavePlayerData(TagCompound tag)
        {
            tag[ScaleKey] = LogScale;
            tag[OffsetKey] = LogPositionOffset;
        }

        public override void LoadPlayerData(TagCompound tag)
        {
            if (tag.TryGet(ScaleKey, out float scale))
            {
                if (LogScale != scale)
                    SetupTargets();

                LogScale = scale;
            }

            if (tag.TryGet(OffsetKey, out Vector2 offset))
                LogPositionOffset = offset;
        }

        #endregion
    }
}

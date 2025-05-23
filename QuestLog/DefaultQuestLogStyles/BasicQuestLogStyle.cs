using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Controls;
using QuestBooks.QuestLog.DefaultQuestLogStyles;
using QuestBooks.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace QuestBooks.QuestLog.DefaultQuestLogStyles
{
    internal class BasicQuestLogStyle : QuestLogStyle
    {
        public override string Key => "DefaultQuestLog";
        public override string DisplayName => "Book";

        public static QuestBook SelectedBook = null;
        public static QuestLine SelectedLine = null;
        public static QuestLineElement SelectedElement = null;

        private static RenderTargetBinding[] returnTargets = null;
        public static RenderTarget2D BooksTarget = null;
        public static RenderTarget2D ChaptersTarget = null;
        public static RenderTarget2D QuestAreaTarget = null;

        #region Mouse Elements

        public static Vector2 ScaledMousePos;
        public static Point MouseCanvas;

        private static bool LeftMouseJustPressed = false;
        private static bool LeftMouseHeld = false;
        private static bool LeftMouseJustReleased = false;

        private static bool RightMouseJustPressed = false;
        private static bool RightMouseHeld = false;
        private static bool RightMouseJustReleased = false;

        #endregion

        #region Canvas Modification Elements

        private static Vector2? CachedMouseClick = null;
        private static bool CanvasMoving = false;
        private static bool CanvasResizing = false;

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
            SetupTargets(Vector2.Zero);
            Main.OnResolutionChanged += SetupTargets;
        }

        public override void OnDeselect()
        {
            Main.OnResolutionChanged -= SetupTargets;
        }

        public override void UpdateLog()
        {
            UpdateMouseClicks();
            DrawTasks.Clear();

            if (!QuestLogDrawer.DisplayLog)
                return;

            // Scale MouseCanvas around the center of the screen.
            //
            // If Main.UIScale is greater than 1, the quest canvas will bleed past the edges of the screen, and
            // if it is less than 1, the canvas will not fill the edges of the screen.
            //
            // MouseCanvas is the position within the bounds of the canvas itself, even if said canvas does not exactly
            // match the bounds of the screen.
            LogArea = CalculateLogArea(out var logSize, out var halfScreen, out var halfRealScreen);
            ScaledMousePos = (Main.MouseScreen - halfScreen) / halfScreen * halfRealScreen + halfRealScreen;
            MouseCanvas = ScaledMousePos.ToPoint();

            // These encompass the entire area covered by quest log UI.
            if (LogArea.Contains(MouseCanvas))
                Main.LocalPlayer.mouseInterface = true;

            UpdateCanvasMovement(halfRealScreen, halfScreen, logSize);
            UpdateCanvasResizing(halfRealScreen);
            UpdateDesignerToggle();

            CalculateLibraryRegions(LogArea, out var books, out var chapters, out var questArea);

            float fadeDesignation = 0.025f;

            SwitchTargets(BooksTarget);
            UpdateBooks(BooksTarget.Bounds.CreateScaledMargins(top: fadeDesignation, bottom: fadeDesignation), (MouseCanvas - books.Location).ToVector2() * (BooksTarget.Size() / books.Size()));
            SwitchTargets(null);

            AddRectangle(books, Color.LimeGreen);
            DrawTasks.Add(sb => sb.Draw(BooksTarget, books.Center(), null, Color.White, 0f, BooksTarget.Size() / 2f, LogScale, SpriteEffects.None, 0f));
            
            SwitchTargets(ChaptersTarget);
            UpdateChapters(ChaptersTarget.Bounds.CreateScaledMargins(top: fadeDesignation, bottom: fadeDesignation));
            SwitchTargets(null);

            AddRectangle(chapters, Color.LimeGreen);
            DrawTasks.Add(sb => sb.Draw(ChaptersTarget, chapters.Center(), null, Color.White, 0f, ChaptersTarget.Size() / 2f, LogScale, SpriteEffects.None, 0f));

            SwitchTargets(QuestAreaTarget);
            UpdateQuestArea(QuestAreaTarget.Bounds.CreateScaledMargin(fadeDesignation));
            SwitchTargets(null);

            AddRectangle(questArea, Color.LimeGreen);
            DrawTasks.Add(sb => sb.Draw(QuestAreaTarget, questArea.Center(), null, Color.White, 0f, QuestAreaTarget.Size() / 2f, LogScale, SpriteEffects.None, 0f));

            if (books.Contains(MouseCanvas) || chapters.Contains(MouseCanvas) || questArea.Contains(MouseCanvas))
                PlayerInput.LockVanillaMouseScroll("QuestBooks/QuestLog");
        }

        public override void DrawLog(SpriteBatch spriteBatch)
        {
            Vector2 halfRealScreen = QuestLogDrawer.RealScreenSize * 0.5f;

            Texture2D logTexture = QuestAssets.BasicQuestCanvas;
            spriteBatch.Draw(logTexture, halfRealScreen + (LogPositionOffset * halfRealScreen), null, Color.White, 0f, logTexture.Size() / 2f, LogScale, SpriteEffects.None, 0f);

            foreach (var drawAction in DrawTasks)
                drawAction(spriteBatch);
        }

        #region Components

        #region Movement

        private static void UpdateDesignerToggle()
        {
            if (QuestBooks.DesignerEnabled)
            {
                Rectangle designerToggle = LogArea.CookieCutter(new(0f, -1.1f), new(0.06f, 0.05f));

                if (designerToggle.Contains(MouseCanvas))
                {
                    Main.LocalPlayer.mouseInterface = true;

                    if (LeftMouseJustPressed)
                    {
                        UseDesigner = !UseDesigner;

                        if (UseDesigner)
                            SoundEngine.PlaySound(SoundID.Item28);

                        else
                            SoundEngine.PlaySound(SoundID.Item78);
                    }
                }

                AddRectangle(designerToggle, Color.Orange);

                if (UseDesigner)
                    AddRectangle(LogArea, Color.Aquamarine);
            }
        }

        private static void UpdateCanvasMovement(Vector2 halfRealScreen, Vector2 halfScreen, Vector2 logSize)
        {
            Rectangle moveTab = LogArea.CookieCutter(new(0f, 0f), new(0.059f, 1f));
            if ((moveTab.Contains(MouseCanvas) || CanvasMoving) && !CanvasResizing)
            {
                // Reset the position if right clicked.
                if (RightMouseJustPressed)
                {
                    LogPositionOffset = Vector2.Zero;
                    CanvasMoving = false;
                    CachedMouseClick = null;
                    return;
                }

                if (!LeftMouseHeld || (!CanvasMoving && !LeftMouseJustPressed))
                {
                    CanvasMoving = false;
                    CachedMouseClick = null;
                    return;
                }

                CanvasMoving = true;

                if (!CachedMouseClick.HasValue)
                    CachedMouseClick = ScaledMousePos;

                else
                {
                    // Move the canvas based on mouse movement and re-size the log area to match.
                    Vector2 mouseMovement = ScaledMousePos - CachedMouseClick.Value;
                    CachedMouseClick = ScaledMousePos;
                    LogPositionOffset += mouseMovement / halfScreen;

                    LogArea = CenteredRectangle(halfRealScreen + (LogPositionOffset * halfRealScreen), logSize);
                }
            }
        }

        private static void UpdateCanvasResizing(Vector2 halfRealScreen)
        {
            Rectangle resizeTab = LogArea.CookieCutter(new(1.01f, 1.02f), new(0.062f, 0.09f));
            if ((resizeTab.Contains(MouseCanvas) || CanvasResizing) && !CanvasMoving)
            {
                Main.LocalPlayer.mouseInterface = true;

                // Reset the scale on right click.
                if (RightMouseJustPressed)
                {
                    LogScale = 1f;
                    CanvasResizing = false;
                    goto PostResize;
                }

                if (!LeftMouseHeld || (!CanvasResizing && !LeftMouseJustPressed))
                {
                    CanvasResizing = false;
                    goto PostResize;
                }

                CanvasResizing = true;

                Vector2 areaLineAngle = LogArea.BottomRight() - LogArea.Center();
                Vector2 mouseLineAngle = areaLineAngle.RotatedBy(-MathHelper.PiOver2);

                Vector2 intersection = GetPointOfIntersection(LogArea.Center(), areaLineAngle, ScaledMousePos, mouseLineAngle);

                if (intersection.X < LogArea.Center().X)
                    goto PostResize;

                Vector2 defaultLogSize = QuestAssets.BasicQuestCanvas.Texture.Size();
                float scale = (intersection - LogArea.Center()).Length() / (defaultLogSize * 0.5f).Length();

                // Minimum scale
                if (scale >= 0.4f)
                    LogScale = scale;

                Vector2 logSize = QuestAssets.BasicQuestCanvas.Texture.Size() * LogScale;
                LogArea = CenteredRectangle(halfRealScreen + (LogPositionOffset * halfRealScreen), logSize);
            }

        PostResize:
            DrawTasks.Add(sb => sb.Draw(QuestAssets.ResizeIndicator, LogArea.BottomRight(), null, Color.White, 0f, QuestAssets.ResizeIndicator.Texture.Size() / 2f, LogScale, SpriteEffects.None, 0f));
        }

        #endregion

        #region Display

        private static void UpdateBooks(Rectangle books, Vector2 scaledMouse)
        {
            var mouseBooks = scaledMouse.ToPoint();

            DrawTasks.Add(sb =>
            {
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);
                sb.DrawRectangle(books, Color.Yellow);
            });
        }

        private static void UpdateChapters(Rectangle chapters)
        {

        }

        private static void UpdateQuestArea(Rectangle questArea)
        {

        }

        #endregion

        #endregion

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
            logSize = QuestAssets.BasicQuestCanvas.Texture.Size() * (scaleOverride ?? LogScale);
            return CenteredRectangle(halfRealScreen + (LogPositionOffset * halfRealScreen), logSize);
        }

        private static void CalculateLibraryRegions(Rectangle logArea, out Rectangle books, out Rectangle chapters, out Rectangle questArea)
        {
            Rectangle library = logArea.CookieCutter(new(-0.505f, -0.055f), new(0.43f, 0.84f)); // The combined area of the Books and Chapters
            questArea = logArea.CookieCutter(new(0.505f, -0.055f), new(0.43f, 0.84f));

            books = library.CookieCutter(new(-0.5f, 0f), new(0.5f, 1f)).CreateMargins(right: 4);
            chapters = library.CookieCutter(new(0.5f, 0f), new(0.5f, 1f)).CreateMargins(left: 4);
        }

        #endregion

        #region Render Targets

        private void SwitchTargets(RenderTarget2D renderTarget, Matrix? matrix = null, Effect effect = null)
        {
            if (renderTarget is null)
            {
                DrawTasks.Add(sb =>
                {
                    sb.End();
                    sb.GraphicsDevice.SetRenderTargets(returnTargets);
                    returnTargets = null;
                    sb.Begin(SpriteSortMode.Deferred, CustomBlendState, CustomSamplerState, CustomDepthStencilState, CustomRasterizerState);
                });

                return;
            }

            DrawTasks.Add(sb =>
            {
                sb.End();
                returnTargets = sb.GraphicsDevice.GetRenderTargets();
                sb.GraphicsDevice.SetRenderTarget(renderTarget);
                sb.Begin(SpriteSortMode.Deferred, CustomBlendState, CustomSamplerState, CustomDepthStencilState, CustomRasterizerState, effect, matrix ?? Matrix.Identity);
            });
        }

        private static void SetupTargets(Vector2 screenSize)
        {
            QuestAssets.BasicQuestCanvas.Value.Wait();
            var basicLogArea = CalculateLogArea(out _, out _, out _, 1f);
            CalculateLibraryRegions(basicLogArea, out Rectangle books, out Rectangle chapters, out Rectangle questArea);

            books.Width -= books.Width % 2;
            books.Height += books.Height % 2;

            chapters.Width -= chapters.Width % 2;
            chapters.Height += chapters.Height % 2;

            questArea.Width -= questArea.Width % 2;
            questArea.Height += questArea.Height % 2;

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

                BooksTarget?.Dispose();
                ChaptersTarget?.Dispose();
                QuestAreaTarget?.Dispose();

                BooksTarget = GenerateTarget(books.Width, books.Height);
                ChaptersTarget = GenerateTarget(chapters.Width, chapters.Height);
                QuestAreaTarget = GenerateTarget(questArea.Width, questArea.Height);
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
                        LogScale = scale;

            if (tag.TryGet(OffsetKey, out Vector2 offset))
                LogPositionOffset = offset;
        }

        private static void UpdateMouseClicks()
        {
            if (Main.mouseLeft)
            {
                if (!LeftMouseHeld)
                    LeftMouseJustPressed = true;

                else
                    LeftMouseJustPressed = false;

                LeftMouseHeld = true;
            }
            else
            {
                if (LeftMouseHeld)
                    LeftMouseJustReleased = true;

                else
                    LeftMouseJustReleased = false;

                LeftMouseHeld = false;
            }

            if (Main.mouseRight)
            {
                if (!RightMouseHeld)
                    RightMouseJustPressed = true;

                else
                    RightMouseJustPressed = false;

                RightMouseHeld = true;
            }
            else
            {
                if (RightMouseHeld)
                    RightMouseJustReleased = true;

                else
                    RightMouseJustReleased = false;

                RightMouseHeld = false;
            }
        }

        #endregion
    }
}

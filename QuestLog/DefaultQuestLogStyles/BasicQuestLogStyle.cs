using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Controls;
using QuestBooks.QuestLog.DefaultQuestBooks;
using QuestBooks.QuestLog.DefaultQuestLines;
using QuestBooks.QuestLog.DefaultQuestLogStyles;
using QuestBooks.Systems;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader.IO;
using static System.Net.Mime.MediaTypeNames;

namespace QuestBooks.QuestLog.DefaultQuestLogStyles
{
    internal class BasicQuestLogStyle : QuestLogStyle
    {
        public override string Key => "DefaultQuestLog";
        public override string DisplayName => "Book";

        private static IEnumerable<BasicQuestBook> availableBooks { get => QuestManager.QuestBooks.Cast<BasicQuestBook>(); }
        private static IEnumerable<BasicQuestLine> availableChapters { get => SelectedBook.Chapters.Cast<BasicQuestLine>(); }

        public static QuestBook SelectedBook = null;
        public static QuestLine SelectedChapter = null;
        public static QuestLineElement SelectedElement = null;

        private static RenderTargetBinding[] returnTargets = null;
        public static RenderTarget2D BooksTarget = null;
        public static RenderTarget2D ChaptersTarget = null;
        public static RenderTarget2D QuestAreaTarget = null;
        private static readonly float FadeDesignation = 0.025f;

        #region Mouse Elements

        public static Vector2 ScaledMousePos;
        public static Point MouseCanvas;

        private static bool LeftMouseJustPressed = false;
        private static bool LeftMouseHeld = false;
        private static bool LeftMouseJustReleased = false;

        private static bool RightMouseJustPressed = false;
        private static bool RightMouseHeld = false;
        private static bool RightMouseJustReleased = false;

        private static string MouseTooltip = "";

        #endregion

        #region Canvas Modification Elements

        private static Vector2? CachedMouseClick = null;
        private static bool CanvasMoving = false;
        private static bool CanvasResizing = false;
        private static float targetScale = 1f;

        private static bool WantsRetarget = false;
        const float scrollAcceleration = 0.2f;

        private static float realBooksScrollOffset = 0;
        private static float realChaptersScrollOffset = 0;

        private static int BooksScrollOffset = 0;
        private static int ChaptersScrollOffset = 0;

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
            BooksTarget?.Dispose();
            ChaptersTarget?.Dispose();
            QuestAreaTarget?.Dispose();
        }

        public override void OnToggle(bool active)
        {

        }

        public override void UpdateLog()
        {
            UpdateMouseClicks();
            DrawTasks.Clear();

            if (!QuestLogDrawer.DisplayLog)
                return;

            if (WantsRetarget)
            {
                SetupTargets();
                WantsRetarget = false;
            }

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
            MouseTooltip = null;

            // These encompass the entire area covered by quest log UI.
            if (LogArea.Contains(MouseCanvas))
            {
                Main.LocalPlayer.mouseInterface = true;
                PlayerInput.LockVanillaMouseScroll("QuestBooks/QuestLog");
                MouseTooltip = "";
            }

            // These handle moving the canvas via dragging the book's spine,
            // resizing with the tab in the bottom-right hand corner,
            // and toggling between the designer and normal log.
            UpdateCanvasMovement(halfRealScreen, halfScreen, logSize);
            UpdateCanvasResizing(halfRealScreen);
            UpdateDesignerToggle();

            // Calculate the area designation for books, chapters, and quest contents.
            // These are scaled and repositioned based on the log's screen position and scale.
            CalculateLibraryRegions(LogArea, out var books, out var chapters, out var questArea);

            //AddRectangle(books, Color.LimeGreen);
            //AddRectangle(chapters, Color.LimeGreen);
            //AddRectangle(questArea, Color.LimeGreen);

            // Render each region to it's own render target to allow for shaders.
            // We use a shader to fade out the edges of each target when drawing to the log.

            UpdateBooks(BooksTarget.Bounds.CreateScaledMargins(top: FadeDesignation, bottom: FadeDesignation), (MouseCanvas - books.Location).ToVector2() * (BooksTarget.Bounds.Size() / books.Size()));            
            UpdateChapters(ChaptersTarget.Bounds.CreateScaledMargins(top: FadeDesignation, bottom: FadeDesignation), (MouseCanvas - chapters.Location).ToVector2() * (ChaptersTarget.Bounds.Size() / chapters.Size()));
            UpdateQuestArea(QuestAreaTarget.Bounds.CreateScaledMargin(FadeDesignation));

            // Render all of the completed render targets with fading applied to the
            // desired edges (top/bottom on books/chapters, all edges on quest area)
            DrawTasks.Add(sb =>
            {
                sb.End();
                QuestAssets.FadedEdges.Asset.Parameters["FadeSides"].SetValue(false);
                sb.Begin(SpriteSortMode.Deferred, CustomBlendState, CustomSamplerState, CustomDepthStencilState, CustomRasterizerState, QuestAssets.FadedEdges);

                float resizeScale = LogScale / targetScale;

                sb.Draw(BooksTarget, books.Center(), null, Color.White, 0f, BooksTarget.Size() * 0.5f, resizeScale, SpriteEffects.None, 0f);
                sb.Draw(ChaptersTarget, chapters.Center(), null, Color.White, 0f, ChaptersTarget.Size() * 0.5f, resizeScale, SpriteEffects.None, 0f);

                sb.End();
                QuestAssets.FadedEdges.Asset.Parameters["FadeSides"].SetValue(true);
                sb.Begin(SpriteSortMode.Deferred, CustomBlendState, CustomSamplerState, CustomDepthStencilState, CustomRasterizerState, QuestAssets.FadedEdges);

                sb.Draw(QuestAreaTarget, questArea.Center(), null, Color.White, 0f, QuestAreaTarget.Size() * 0.5f, resizeScale, SpriteEffects.None, 0f);

                sb.End();
                sb.Begin(SpriteSortMode.Deferred, CustomBlendState, CustomSamplerState, CustomDepthStencilState, CustomRasterizerState);
            });

            if (MouseTooltip is not null)
                Main.instance.MouseText(MouseTooltip, "", noOverride: true);
        }

        // Draw the log texture before running through all of our queued
        // draw tasks from update. This allows us to practically handle
        // updating and drawing at the same time, while skipping over
        // performing the actual draw work when it is not necessary.
        public override void DrawLog(SpriteBatch spriteBatch)
        {
            Vector2 halfRealScreen = QuestLogDrawer.RealScreenSize * 0.5f;

            Texture2D logTexture = QuestAssets.BasicQuestCanvas;
            spriteBatch.Draw(logTexture, halfRealScreen + (LogPositionOffset * halfRealScreen), null, Color.White, 0f, logTexture.Size() * 0.5f, LogScale, SpriteEffects.None, 0f);

            foreach (var drawAction in DrawTasks)
                drawAction(spriteBatch);
        }

        #region Components

        #region Display

        private static readonly List<(Rectangle area, BasicQuestBook questBook)> bookLibrary = [];
        private void UpdateBooks(Rectangle books, Vector2 scaledMouse)
        {
            SwitchTargets(BooksTarget);
            DrawTasks.Add(_ => Main.graphics.GraphicsDevice.Clear(Color.Black * 0.1f));

            if (!availableBooks.Any())
            {
                SwitchTargets(null);
                return;
            }

            var mouseBooks = scaledMouse.ToPoint();

            bookLibrary.Clear();
            Rectangle book = books.CookieCutter(new(0f, -0.9f), new(1f, 0.1f));
            bookLibrary.Add((book, availableBooks.ElementAt(0)));

            for (int i = 1; i < availableBooks.Count(); i++)
            {
                book = book.CookieCutter(new(0f, 2.5f), Vector2.One);
                bookLibrary.Add((book, availableBooks.ElementAt(i)));
            }

            if (books.Contains(mouseBooks))
            {
                int data = PlayerInput.ScrollWheelDeltaForUI;

                if (data != 0)
                {
                    int scrollAmount = data / 6;
                    BooksScrollOffset += scrollAmount;

                    Rectangle lastBook = bookLibrary[^1].area;
                    int minScrollValue = -(lastBook.Bottom - (books.Height + books.Y));

                    if (minScrollValue < 0)
                        BooksScrollOffset = int.Clamp(BooksScrollOffset, minScrollValue, 0);

                    else
                        BooksScrollOffset = 0;
                }
            }

            realBooksScrollOffset = MathHelper.Lerp(realBooksScrollOffset, BooksScrollOffset, scrollAcceleration);

            foreach ((var rectangle, var questBook) in bookLibrary)
            {
                rectangle.Offset(0, (int)realBooksScrollOffset);
                bool hovered = rectangle.Contains(mouseBooks);

                if (hovered && LeftMouseJustReleased)
                {
                    if (SelectedBook != questBook)
                        SelectedBook = questBook;

                    else
                        SelectedBook = null;
                    
                    ChaptersScrollOffset = 0;
                    realChaptersScrollOffset = 0f;
                }

                DrawTasks.Add(sb =>
                {
                    Color color = Color.SlateGray;

                    if (SelectedBook != questBook)
                        color = Color.Lerp(color, Color.Black, 0.25f);

                    if (hovered)
                        color = Color.Lerp(color, Color.White, 0.1f);

                    Color outlineColor = Color.Lerp(color, Color.Black, 0.2f);
                    sb.Draw(QuestAssets.LogEntryBackground, rectangle.Center(), null, color, 0f, QuestAssets.LogEntryBackground.Asset.Size() * 0.5f, targetScale, SpriteEffects.None, 0f);
                    sb.Draw(QuestAssets.LogEntryBorder, rectangle.Center(), null, outlineColor, 0f, QuestAssets.LogEntryBorder.Asset.Size() * 0.5f, targetScale, SpriteEffects.None, 0f);
                    outlineColor = Color.Lerp(outlineColor, Color.Black, 0.4f);

                    string displayName = questBook.DisplayName;
                    Rectangle nameRectangle = rectangle.CreateScaledMargins(left: 0.065f, right: 0.165f, top: 0.1f, bottom: 0.1f);

                    float scaleShift = InverseLerp(0.4f, 2f, LogScale) * 0.8f;
                    float stroke = MathHelper.Lerp(1f, 4f, scaleShift);
                    Vector2 offset = new(0f, MathHelper.Lerp(2f, 10f, scaleShift));

                    var font = FontAssets.DeathText.Value;
                    var (line, drawPos, origin, scale) = GetRectangleStringParameters(nameRectangle, font, displayName, offset: offset, alignment: Utilities.TextAlignment.Left)[0];

                    DynamicSpriteFontExtensionMethods.DrawString(sb, font, line, drawPos + new Vector2(-stroke, 0f), outlineColor, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                    DynamicSpriteFontExtensionMethods.DrawString(sb, font, line, drawPos + new Vector2(stroke, 0f), outlineColor, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                    DynamicSpriteFontExtensionMethods.DrawString(sb, font, line, drawPos + new Vector2(0f, stroke), outlineColor, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                    DynamicSpriteFontExtensionMethods.DrawString(sb, font, line, drawPos + new Vector2(0f, -stroke), outlineColor, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                    DynamicSpriteFontExtensionMethods.DrawString(sb, font, line, drawPos, Color.White, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                });
            }

            SwitchTargets(null);
        }

        private static readonly List<(Rectangle area, BasicQuestLine questBook)> chapterLibrary = [];
        private void UpdateChapters(Rectangle chapters, Vector2 scaledMouse)
        {
            SwitchTargets(ChaptersTarget);
            DrawTasks.Add(_ => Main.graphics.GraphicsDevice.Clear(Color.Black * 0.1f));

            if (SelectedBook is null || !availableChapters.Any())
            {
                SwitchTargets(null);
                return;
            }

            var mouseChapters = scaledMouse.ToPoint();
            bool hoveringChapters = false;

            chapterLibrary.Clear();
            Rectangle chapter = chapters.CookieCutter(new(0f, -0.9f), new(1f, 0.1f));
            chapterLibrary.Add((chapter, availableChapters.ElementAt(0)));

            for (int i = 1; i < availableChapters.Count(); i++)
            {
                chapter = chapter.CookieCutter(new(0f, 2.5f), Vector2.One);
                chapterLibrary.Add((chapter, availableChapters.ElementAt(i)));
            }

            if (chapters.Contains(mouseChapters))
            {
                hoveringChapters = true;
                int data = PlayerInput.ScrollWheelDeltaForUI;

                if (data != 0)
                {
                    int scrollAmount = (int)(data / 2.5f);
                    ChaptersScrollOffset += scrollAmount;

                    Rectangle lastBook = chapterLibrary[^1].area;
                    int minScrollValue = -(lastBook.Bottom - (chapters.Height + chapters.Y));

                    if (minScrollValue < 0)
                        ChaptersScrollOffset = int.Clamp(ChaptersScrollOffset, minScrollValue, 0);

                    else
                        ChaptersScrollOffset = 0;
                }
            }

            realChaptersScrollOffset = MathHelper.Lerp(realChaptersScrollOffset, ChaptersScrollOffset, scrollAcceleration);

            foreach ((var rectangle, var questLine) in chapterLibrary)
            {
                rectangle.Offset(0, (int)realChaptersScrollOffset);
                bool hovered = hoveringChapters && rectangle.Contains(mouseChapters);

                if (hovered && LeftMouseJustReleased)
                {
                    if (SelectedChapter != questLine)
                        SelectedChapter = questLine;

                    else
                        SelectedChapter = null;

                    //QuestAreaPositionOffset = Vector2.Zero;
                }

                DrawTasks.Add(sb =>
                {
                    Color color = Color.MediumSeaGreen;

                    if (SelectedChapter != questLine)
                        color = Color.Lerp(color, Color.Black, 0.35f);

                    if (hovered)
                        color = Color.Lerp(color, Color.White, 0.1f);

                    Color outlineColor = Color.Lerp(color, Color.Black, 0.2f);
                    sb.Draw(QuestAssets.LogEntryBackground, rectangle.Center(), null, color, 0f, QuestAssets.LogEntryBackground.Asset.Size() * 0.5f, targetScale, SpriteEffects.None, 0f);
                    sb.Draw(QuestAssets.LogEntryBorder, rectangle.Center(), null, outlineColor, 0f, QuestAssets.LogEntryBorder.Asset.Size() * 0.5f, targetScale, SpriteEffects.None, 0f);
                    outlineColor = Color.Lerp(outlineColor, Color.Black, 0.4f);

                    string displayName = questLine.DisplayName;
                    Rectangle nameRectangle = rectangle.CreateScaledMargins(left: 0.065f, right: 0.165f, top: 0.1f, bottom: 0.1f);

                    float scaleShift = InverseLerp(0.4f, 2f, LogScale) * 0.8f;
                    float stroke = MathHelper.Lerp(1f, 4f, scaleShift);
                    Vector2 offset = new(0f, MathHelper.Lerp(2f, 10f, scaleShift));

                    var font = FontAssets.DeathText.Value;
                    var (line, drawPos, origin, scale) = GetRectangleStringParameters(nameRectangle, font, displayName, offset: offset, alignment: Utilities.TextAlignment.Left)[0];

                    sb.End();
                    sb.Begin(SpriteSortMode.Deferred, CustomBlendState, SamplerState.LinearClamp, CustomDepthStencilState, CustomRasterizerState);

                    DynamicSpriteFontExtensionMethods.DrawString(sb, font, line, drawPos + new Vector2(-stroke, 0f), outlineColor, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                    DynamicSpriteFontExtensionMethods.DrawString(sb, font, line, drawPos + new Vector2(stroke, 0f), outlineColor, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                    DynamicSpriteFontExtensionMethods.DrawString(sb, font, line, drawPos + new Vector2(0f, stroke), outlineColor, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                    DynamicSpriteFontExtensionMethods.DrawString(sb, font, line, drawPos + new Vector2(0f, -stroke), outlineColor, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                    DynamicSpriteFontExtensionMethods.DrawString(sb, font, line, drawPos, Color.White, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);

                    sb.End();
                    sb.Begin(SpriteSortMode.Deferred, CustomBlendState, CustomSamplerState, CustomDepthStencilState, CustomRasterizerState);
                });
            }

            SwitchTargets(null);
        }

        private void UpdateQuestArea(Rectangle questArea)
        {
            SwitchTargets(QuestAreaTarget);
            DrawTasks.Add(_ => Main.graphics.GraphicsDevice.Clear(Color.Gray * 0.12f));
            SwitchTargets(null);
        }

        #endregion

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
                    LogPositionOffset += mouseMovement / halfRealScreen;

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
                    WantsRetarget = true;
                    goto PostResize;
                }

                if (!LeftMouseHeld || (!CanvasResizing && !LeftMouseJustPressed))
                {
                    if (CanvasResizing)
                        WantsRetarget = true;

                    CanvasResizing = false;
                    goto PostResize;
                }

                CanvasResizing = true;

                Vector2 areaLineAngle = LogArea.BottomRight() - LogArea.Center();
                Vector2 mouseLineAngle = areaLineAngle.RotatedBy(-MathHelper.PiOver2);

                Vector2 intersection = GetPointOfIntersection(LogArea.Center(), areaLineAngle, ScaledMousePos, mouseLineAngle);

                if (intersection.X < LogArea.Center().X)
                    goto PostResize;

                Vector2 defaultLogSize = QuestAssets.BasicQuestCanvas.Asset.Size();
                float scale = (intersection - LogArea.Center()).Length() / (defaultLogSize * 0.5f).Length();

                // Clamped scale
                if (scale >= 0.4f && scale <= 2f)
                    LogScale = scale;

                Vector2 logSize = QuestAssets.BasicQuestCanvas.Asset.Size() * LogScale;
                LogArea = CenteredRectangle(halfRealScreen + (LogPositionOffset * halfRealScreen), logSize);
            }

        PostResize:
            DrawTasks.Add(sb => sb.Draw(QuestAssets.ResizeIndicator, LogArea.BottomRight(), null, Color.White, 0f, QuestAssets.ResizeIndicator.Asset.Size() * 0.5f, LogScale, SpriteEffects.None, 0f));
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
            logSize = QuestAssets.BasicQuestCanvas.Asset.Size() * (scaleOverride ?? LogScale);
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

        private static void SetupTargets()
        {
            QuestAssets.BasicQuestCanvas.Value.Wait();
            var basicLogArea = CalculateLogArea(out _, out _, out _, LogScale);
            targetScale = LogScale;
            CalculateLibraryRegions(basicLogArea, out Rectangle books, out Rectangle chapters, out Rectangle questArea);

            books.Width = books.Width.ToNearestDoubleEven();
            books.Height = books.Height.ToNearestDoubleEven();

            chapters.Width = chapters.Width.ToNearestDoubleEven();
            chapters.Height = chapters.Height.ToNearestDoubleEven();

            questArea.Width = questArea.Width.ToNearestDoubleEven();
            questArea.Height = questArea.Height.ToNearestDoubleEven();

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

                var oldBooks = BooksTarget;
                var oldChapters = ChaptersTarget;
                var oldQuests = QuestAreaTarget;

                BooksTarget = GenerateTarget(books.Width, books.Height);
                ChaptersTarget = GenerateTarget(chapters.Width, chapters.Height);
                QuestAreaTarget = GenerateTarget(questArea.Width, questArea.Height);

                oldBooks?.Dispose();
                oldChapters?.Dispose();
                oldQuests?.Dispose();
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

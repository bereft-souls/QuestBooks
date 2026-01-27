using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace QuestBooks.QuestLog.DefaultStyles
{
    public partial class BasicQuestLogStyle : QuestLogStyle
    {
        public override string Key => "DefaultQuestLog";
        public override string DisplayName => "Book";

        public QuestLogElement[] SortedElements { get; set; } = null;

        // Mouse position on canvas
        protected Vector2 ScaledMousePos { get; set; }
        protected Point MouseCanvas { get; set; }

        protected bool PreviouslyOpened { get; set; } = false;

        // Indicates whether ANY part of the log has just been moved
        // Could be the log itself, could be resizing, could be the quest area
        // Used to keep track of whether the right click was intended to "reset" the current action, or the result of some other action
        protected bool JustMoved { get; set; } = false;

        // Our blending drastically changes between content draws
        protected static BlendState LayerBlending { get; } = new()
        {
            ColorSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.One,
            AlphaDestinationBlend = Blend.One,
            AlphaBlendFunction = BlendFunction.Add,
        };

        protected static BlendState ContentBlending { get; } = new()
        {
            ColorSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.One,
            AlphaBlendFunction = BlendFunction.Add
        };

        protected static BlendState TargetCopying { get; } = new()
        {
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.One,
            AlphaDestinationBlend = Blend.One,
            AlphaBlendFunction = BlendFunction.Add,
        };

        protected static BlendState LibraryBlending { get; } = new()
        {
            ColorSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.DestinationAlpha,
            AlphaBlendFunction = BlendFunction.Max
        };

        protected static BlendState GridBlending { get; } = new()
        {
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.One,
            ColorBlendFunction = BlendFunction.Max,
            AlphaSourceBlend = Blend.One,
            AlphaDestinationBlend = Blend.One,
            AlphaBlendFunction = BlendFunction.Max
        };

        private RenderTargetBinding[] returnTargets = null;
        private BlendState returnBlend = null;
        private SamplerState returnSampler = null;
        private DepthStencilState returnDepth = null;
        private RasterizerState returnRaster = null;
        private Effect returnEffect = null;
        private Matrix returnMatrix = Matrix.Identity;

        private RenderTarget2D booksTarget = null;
        private RenderTarget2D chaptersTarget = null;
        private RenderTarget2D libraryTarget = null;

        private RenderTarget2D questInfoTarget = null;
        private RenderTarget2D previousQuestInfoTarget = null;

        private RenderTarget2D questAreaTarget = null;
        private RenderTarget2D previousQuestAreaTarget = null;

        private bool swipingBetweenInfoPages = false;
        private float questInfoSwipeOffset = 0f;
        private const float FadeDesignation = 0.024f;

        private bool onCoverPage;
        private int pageFlippingTimer;

        #region Element Referentials

        private bool wantsRetarget = true;
        private const float scrollAcceleration = 0.2f;

        #endregion

        #region Draw Parameters

        public Vector2 LogPositionOffset { get; set; } = Vector2.Zero;
        public float LogScale { get; set; } = 1f;

        private Rectangle LogArea;
        private readonly List<Action<SpriteBatch>> DrawTasks = [];

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
            libraryTarget?.Dispose();
            questAreaTarget?.Dispose();
            previousQuestAreaTarget?.Dispose();
            questInfoTarget?.Dispose();
            previousQuestInfoTarget?.Dispose();
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
            Vector2 questLogCenter = halfRealScreen + (LogPositionOffset * halfRealScreen);
            UpdateMousePosition(halfScreen, halfRealScreen);

            if (!PreviouslyOpened)
            {
                if (DrawBookOpening(questLogCenter))
                    PreviouslyOpened = true;

                QuestLogDrawer.OpenTimer = 0;
                QuestLogDrawer.QuestLogDrawOpacity = 1f;
                QuestLogDrawer.QuestLogDrawOffset = Vector2.Zero;
                return;
            }

            if (QuestLogDrawer.OpenTimer > 0)
            {
                LeftMouseHeld = false;
                LeftMouseJustPressed = false;
                LeftMouseJustReleased = false;
                RightMouseHeld = false;
                RightMouseJustPressed = false;
                RightMouseJustReleased = false;

                ScaledMousePos = new(-10000f, -10000f);
                MouseCanvas = new(-10000, -10000);
            }

            if (pageFlippingTimer > 0)
            {
                pageFlippingTimer--;

                DrawTasks.Add(sb =>
                {
                    Texture2D cover = QuestAssets.ClosedBook;
                    sb.Draw(cover, questLogCenter, null, Color.White, 0f, cover.Size() * 0.5f, LogScale, SpriteEffects.None, 0f);
                    return;

                    Texture2D flippingTexture = null;
                    Rectangle frame = new(0, 0, 0, 0);
                    sb.Draw(flippingTexture, questLogCenter, frame, Color.White, 0f, frame.Size() * 0.5f, LogScale, SpriteEffects.None, 0f);
                });

                return;
            }

            if (onCoverPage)
            {
                HandleBookCover(questLogCenter);
                return;
            }

            Texture2D logTexture = QuestAssets.QuestLogCanvas;
            DrawTasks.Add(sb => sb.Draw(logTexture, questLogCenter, null, Color.White, 0f, logTexture.Size() * 0.5f, LogScale, SpriteEffects.None, 0f));

            // These handle moving the canvas via dragging the book's spine,
            // resizing with the tab in the bottom-right hand corner,
            // and toggling between the designer and normal log.
            JustMoved = false;
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

            // We draw an "info page" if the selected element has one, or if we're in the designer
            // The designer info page allows us to modify members of some elements
            bool infoPage = SelectedElement is not null && (SelectedElement.HasInfoPage || UseDesigner);

            if (infoPage)
            {
                Matrix transform = Matrix.CreateScale(TargetScale);
                SwitchTargets(questInfoTarget, matrix: transform, blend: BlendState.AlphaBlend);
                DrawTasks.Add(sb => sb.GraphicsDevice.Clear(Color.Transparent));
                Vector2 infoMousePosition = (MouseCanvas - questInfo.Location).ToVector2() * (questInfoTarget.Bounds.Size() / questInfo.Size()) / TargetScale;

                if (UseDesigner)
                    HandleElementProperties(new Rectangle(0, 0, (int)(questInfoTarget.Width / TargetScale), (int)(questInfoTarget.Height / TargetScale)), infoMousePosition);

                else
                    DrawTasks.Add(sb =>
                    {
                        Action layerAction = null;
                        SelectedElement.DrawInfoPage(sb, infoMousePosition, ref layerAction);
                        ExtraInferfaceLayerMods.Add(layerAction);
                    });

                SwitchTargets(null);

                if (previousBookSwipeOffset == 0)
                {
                    SwitchTargets(previousQuestInfoTarget, BlendState.AlphaBlend);
                    DrawTasks.Add(sb => sb.GraphicsDevice.Clear(Color.Transparent));
                    DrawTasks.Add(sb => sb.Draw(questInfoTarget, previousQuestInfoTarget.Bounds.Center(), null, Color.White, 0f, questInfoTarget.Size() * 0.5f, 1f, SpriteEffects.None, 0f));
                    SwitchTargets(null);
                }
            }

            // Render all of the completed render targets with fading applied to the
            // desired edges (top/bottom on books/chapters, all edges on quest area)
            DrawTasks.Add(sb =>
            {
                sb.End();
                var fadedEdges = QuestAssets.FadedEdges.Asset;
                fadedEdges.Parameters["FadeLeft"].SetValue(false);
                fadedEdges.Parameters["FadeRight"].SetValue(false);

                var targets = sb.GraphicsDevice.GetRenderTargets();
                sb.GraphicsDevice.SetRenderTarget(libraryTarget);
                sb.GraphicsDevice.Clear(Color.Transparent);
                sb.Begin(SpriteSortMode.Deferred, LayerBlending, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, fadedEdges);

                float resizeScale = LogScale / TargetScale;
                books = libraryTarget.Bounds.CookieCutter(new(-0.5f, 0f), new(0.5f, 1f)).CreateMargins(right: 4);
                chapters = libraryTarget.Bounds.CookieCutter(new(0.5f, 0f), new(0.5f, 1f)).CreateMargins(left: 4);

                if (questInfoSwipeOffset != 0f)
                {
                    bool farFromEdge = Math.Abs(questInfoSwipeOffset) > libraryTarget.Height * FadeDesignation * 0.5f;
                    fadedEdges.Parameters["FadeTop"].SetValue(false);
                    fadedEdges.Parameters["FadeBottom"].SetValue(farFromEdge);

                    questInfoSwipeOffset = MathHelper.Lerp(questInfoSwipeOffset, 0f, 0.2f);

                    float libraryOffset = -float.Sign(questInfoSwipeOffset) * (questInfoTarget.Height - Math.Abs(questInfoSwipeOffset));
                    float questInfoOffset = questInfoSwipeOffset;

                    if (!infoPage)
                        (libraryOffset, questInfoOffset) = (questInfoOffset, libraryOffset);

                    if (swipingBetweenInfoPages)
                    {
                        sb.Draw(previousQuestInfoTarget, libraryTarget.Bounds.Center() + new Vector2(0f, libraryOffset), null, Color.White, 0f, previousQuestInfoTarget.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
                    }

                    else
                    {
                        Vector2 offset = new(0f, libraryOffset);
                        sb.Draw(booksTarget, books.Center() + offset, null, Color.White, 0f, booksTarget.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
                        sb.Draw(chaptersTarget, chapters.Center() + offset, null, Color.White, 0f, chaptersTarget.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
                    }

                    sb.End();
                    fadedEdges.Parameters["FadeTop"].SetValue(farFromEdge);
                    fadedEdges.Parameters["FadeBottom"].SetValue(false);
                    sb.Begin(SpriteSortMode.Deferred, LayerBlending, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, fadedEdges);

                    sb.Draw(questInfoTarget, libraryTarget.Bounds.Center() + new Vector2(0f, questInfoOffset), null, Color.White, 0f, questInfoTarget.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

                    if (Math.Abs(questInfoSwipeOffset) < 0.05f)
                        questInfoSwipeOffset = 0f;
                }

                else if (infoPage)
                {
                    fadedEdges.Parameters["FadeTop"].SetValue(false);
                    fadedEdges.Parameters["FadeBottom"].SetValue(false);
                    sb.Draw(questInfoTarget, libraryTarget.Bounds.Center(), null, Color.White, 0f, questInfoTarget.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
                }

                else
                {
                    fadedEdges.Parameters["FadeTop"].SetValue(false);
                    fadedEdges.Parameters["FadeBottom"].SetValue(false);

                    sb.Draw(booksTarget, books.Center(), null, Color.White, 0f, booksTarget.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
                    sb.Draw(chaptersTarget, chapters.Center(), null, Color.White, 0f, chaptersTarget.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
                }

                sb.End();
                fadedEdges.Parameters["FadeTop"].SetValue(true);
                fadedEdges.Parameters["FadeBottom"].SetValue(true);
                sb.GraphicsDevice.SetRenderTargets(targets);
                sb.Begin(SpriteSortMode.Deferred, LayerBlending, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, fadedEdges);

                sb.Draw(libraryTarget, questInfo.Center(), null, Color.White, 0f, libraryTarget.Size() * 0.5f, resizeScale, SpriteEffects.None, 0f);

                sb.End();
                fadedEdges.Parameters["FadeRight"].SetValue(true);
                fadedEdges.Parameters["FadeLeft"].SetValue(true);
                sb.Begin(SpriteSortMode.Deferred, LayerBlending, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, fadedEdges);

                sb.Draw(questAreaTarget, questArea.Center(), null, Color.White, 0f, questAreaTarget.Size() * 0.5f, resizeScale, SpriteEffects.None, 0f);

                sb.End();
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            });

            if (UseDesigner)
                UpdateDesigner(books, chapters, questArea);

            else
                HandleCoverToggle();
        }

        // This allows us to practically handle updating and drawing at the same time,
        // while skipping over performing the actual draw work when it is not necessary.
        public override void DrawLog(SpriteBatch spriteBatch)
        {
            foreach (var drawAction in DrawTasks)
                drawAction(spriteBatch);
        }

        // Set in LoadPlayerData()
        private float? verticalDrawPos;
        private int frozenFrames;
        private float bookRotation;

        // Draws the book animation opening.
        // Returns true when the animation is complete.
        protected virtual bool DrawBookOpening(Vector2 questLogCenter)
        {
            // Assign initial draw position just off the bottom of the screen.
            // This is immediately lerped into the screen bounds.
            verticalDrawPos ??= QuestLogDrawer.RealScreenSize.Y + (QuestAssets.ClosedBook.Asset.Height * 0.5f);
            float targetDrawHeight = questLogCenter.Y;

            if (verticalDrawPos.Value != targetDrawHeight)
            {
                verticalDrawPos = float.Lerp(verticalDrawPos.Value, targetDrawHeight, 0.09f);

                if (verticalDrawPos.Value - targetDrawHeight < 0.25f)
                    verticalDrawPos = targetDrawHeight;
            }
            else
            {
                frozenFrames--;
            }

            if (bookRotation != 0f)
            {
                bookRotation = float.Lerp(bookRotation, 0f, 0.09f);
                if (bookRotation < 0.001f)
                    bookRotation = 0f;
            }

            DrawTasks.Add(spriteBatch =>
            {
                Vector2 drawPos = new(questLogCenter.X, verticalDrawPos.Value);
                Texture2D closedBook = QuestAssets.ClosedBook;
                spriteBatch.Draw(closedBook, drawPos, null, Color.White, bookRotation, closedBook.Size() * 0.5f, LogScale, SpriteEffects.None, 0f);
                QuestLogDrawer.CoverDrawCalls[QuestManager.ActiveQuestLog].Invoke(spriteBatch, drawPos, LogScale, bookRotation);
            });

            return frozenFrames == 0;
        }

        public static void DrawDefaultCover(SpriteBatch spriteBatch, Vector2 drawPos, float scale, float rotation)
        {
            Texture2D tree = QuestAssets.CoverTree;
            spriteBatch.Draw(tree, drawPos, null, Color.White, rotation, tree.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

        public static string RetrieveDefaultLogTitle(string questLogKey)
        {
            string localizationKey = $"Mods.{QuestManager.QuestLogMods[questLogKey].Name}.QuestBooks.{questLogKey}.Name";
            return Language.Exists(localizationKey) ? Language.GetTextValue(localizationKey) : questLogKey;
        }

        public static void DrawDefaultLogTitle(SpriteBatch spriteBatch, Rectangle drawArea, string title, float opacity, bool hovered, bool selected)
        {
            //spriteBatch.DrawRectangle(drawArea, hovered ? Color.Yellow : Color.LightBlue, fill: true);
            spriteBatch.DrawOutlinedStringInRectangle(drawArea, FontAssets.DeathText.Value, hovered ? Color.Yellow : Color.White, Color.Black, title, extraScale: 1.2f, clipBounds: false, offset: new(0f, 12f));
        }

        public override void SelectBook(QuestBook book)
        {
            if (book == SelectedBook)
                return;

            if (book == null)
                DrawTasks.Add(_ => Select());

            else
                Select();

            void Select()
            {
                previousBook = SelectedBook;
                SelectedBook = book;
                SoundEngine.PlaySound(SoundID.MenuTick);

                previousChapterScrollOffset = (int)realChaptersScrollOffset;
                chaptersScrollOffset = 0;
                realChaptersScrollOffset = 0f;

                previousBookSwipeDirection = SelectedBook is null || bookLibrary.FindIndex(kvp => kvp.questBook == SelectedBook) > bookLibrary.FindIndex(kvp => kvp.questBook == previousBook);
                previousBookSwipeOffset = 1f;
            }
        }

        public override void SelectChapter(QuestChapter chapter)
        {
            if (chapter == SelectedChapter)
                return;

            if (chapter == null)
                DrawTasks.Add(_ => Select());

            else
                Select();

            void Select()
            {
                int sign = SelectedBook.Chapters.IndexOf(chapter ?? SelectedChapter) >= SelectedBook.Chapters.IndexOf(SelectedChapter) ? 1 : -1;
                questElementSwipeOffset = questAreaTarget.Width * sign;
                SortedElements = null;

                SelectedChapter = chapter;
                Zoom = (SelectedChapter?.EnableShifting ?? false) ? SelectedChapter.DefaultZoom : 1f;
                QuestAreaOffset = (SelectedChapter?.EnableShifting ?? false) ? SelectedChapter.ViewAnchor * Zoom - defaultAnchor : Vector2.Zero;
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
        }

        public override void OnEnterWorld() => SortedElements = null;

        #region Area Calculation

        // Queues a rectangle for drawing.
        private Rectangle AddRectangle(Rectangle rectangle, Color color, float stroke = 2f, bool fill = false)
        {
            DrawTasks.Add(sb => sb.DrawRectangle(rectangle, color, stroke, fill));
            return rectangle;
        }

        private Rectangle CalculateLogArea(out Vector2 logSize, out Vector2 halfScreen, out Vector2 halfRealScreen, float? scaleOverride = null)
        {
            halfScreen = Main.ScreenSize.ToVector2() * 0.5f;
            halfRealScreen = QuestLogDrawer.RealScreenSize * 0.5f;
            logSize = QuestAssets.QuestLogCanvas.Asset.Size() * (scaleOverride ?? LogScale);
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

        private void SwitchTargets(RenderTarget2D renderTarget,
            BlendState blend = null,
            SamplerState sampler = null,
            DepthStencilState depth = null,
            RasterizerState raster = null,
            Effect effect = null,
            Matrix? matrix = null)
        {
            if (renderTarget is null)
            {
                DrawTasks.Add(sb =>
                {
                    sb.End();
                    sb.GraphicsDevice.SetRenderTargets(returnTargets);
                    returnTargets = null;
                    sb.Begin(SpriteSortMode.Deferred, blend ?? returnBlend, sampler ?? returnSampler, depth ?? returnDepth, raster ?? returnRaster, effect ?? returnEffect, matrix ?? returnMatrix);
                });

                return;
            }

            DrawTasks.Add(sb =>
            {
                sb.End();
                sb.GetDrawParameters(out returnBlend, out returnSampler, out returnDepth, out returnRaster, out returnEffect, out returnMatrix);
                returnTargets = sb.GraphicsDevice.GetRenderTargets();
                sb.GraphicsDevice.SetRenderTarget(renderTarget);
                sb.Begin(SpriteSortMode.Deferred, blend ?? BlendState.AlphaBlend, sampler ?? SamplerState.LinearClamp, depth ?? DepthStencilState.Default, raster ?? RasterizerState.CullNone, effect ?? null, matrix ?? Matrix.Identity);
            });
        }

        private void SetupTargets()
        {
            QuestAssets.QuestLogCanvas.Value.Wait();
            var basicLogArea = CalculateLogArea(out _, out _, out _, LogScale);
            TargetScale = LogScale;
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
                var oldLibrary = libraryTarget;
                var oldInfo = questInfoTarget;
                var oldPreviousInfo = previousQuestInfoTarget;
                var oldQuests = questAreaTarget;
                var oldPreviousQuests = previousQuestAreaTarget;

                booksTarget = GenerateTarget(books.Width, books.Height);
                chaptersTarget = GenerateTarget(chapters.Width, chapters.Height);
                libraryTarget = GenerateTarget(questInfo.Width, questInfo.Height);
                questInfoTarget = GenerateTarget(questInfo.Width, questInfo.Height);
                previousQuestInfoTarget = GenerateTarget(questInfo.Width, questInfo.Height);
                questAreaTarget = GenerateTarget(questArea.Width, questArea.Height);
                previousQuestAreaTarget = GenerateTarget(questArea.Width, questArea.Height);

                oldBooks?.Dispose();
                oldChapters?.Dispose();
                oldLibrary?.Dispose();
                oldInfo?.Dispose();
                oldPreviousInfo?.Dispose();
                oldQuests?.Dispose();
                oldPreviousQuests?.Dispose();
            }

            if (ThreadCheck.IsMainThread)
                TargetSetup();

            else
                Main.RunOnMainThread(TargetSetup);
        }

        #endregion

        #region State Caching

        private const string ScaleKey = "QuestBooks:QuestBooksScale";
        private const string OffsetKey = "QuestBooks:QuestBooksOffset";

        public override void SavePlayerData(TagCompound tag)
        {
            tag[ScaleKey] = LogScale;
            tag[OffsetKey] = LogPositionOffset;
        }

        public override void LoadPlayerData(TagCompound tag)
        {
            if (tag.TryGet(ScaleKey, out float scale) && LogScale != scale)
            {
                LogScale = scale;
                SetupTargets();
            }

            if (tag.TryGet(OffsetKey, out Vector2 offset))
                LogPositionOffset = offset;

            PreviouslyOpened = false;
            verticalDrawPos = null;
            frozenFrames = 60;
            bookRotation = MathHelper.PiOver4;

            onCoverPage = false;
            pageFlippingTimer = 60;
        }

        #endregion
    }
}

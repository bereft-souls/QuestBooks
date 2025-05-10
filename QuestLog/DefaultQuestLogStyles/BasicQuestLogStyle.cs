using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Controls;
using QuestBooks.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace QuestBooks.QuestLog.DefaultQuestLogStyles
{
    internal class BasicQuestLogStyle : QuestLogStyle
    {
        public override string Key => "DefaultQuestLog";
        public override string DisplayName => "Book";

        public static bool DebugDisplay = false;

        public static QuestBook SelectedBook = null;
        public static QuestLine SelectedLine = null;
        public static QuestLineElement SelectedElement = null;

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

        #region Draw Regions

        private static Rectangle LogArea;

        private static Rectangle Books;
        private static Rectangle Chapters;
        private static Rectangle Canvas;

        private static Rectangle MoveTab;
        private static Rectangle ResizeTab;

        private static Rectangle DesignerToggle;

        #endregion

        #region Normal Log

        public override void UpdateLog()
        {
            UpdateMouseClicks();

            if (!QuestLogDrawer.DisplayLog)
                return;

            //QuestLogDrawer.LogPositionOffset = Vector2.Zero;

            // Scale MouseCanvas around the center of the screen.
            //
            // If Main.UIScale is greater than 1, the quest canvas will bleed past the edges of the screen, and
            // if it is less than 1, the canvas will not fill the edges of the screen.
            //
            // MouseCanvas is the position within the bounds of the canvas itself, even if said canvas does not exactly
            // match the bounds of the screen.
            Vector2 halfScreen = Main.ScreenSize.ToVector2() * 0.5f;
            Vector2 halfRealScreen = QuestLogDrawer.RealScreenSize * 0.5f;

            ScaledMousePos = (Main.MouseScreen - halfScreen) / halfScreen / Main.UIScale * halfRealScreen + halfRealScreen;
            MouseCanvas = ScaledMousePos.ToPoint();

            // Calculate a log area pre-movement check.
            // This will be recalculated if the canvas moves.
            Vector2 logSize = QuestAssets.BasicQuestCanvas.Texture.Size() * QuestLogDrawer.LogScale;
            LogArea = CenteredRectangle(halfRealScreen + (QuestLogDrawer.LogPositionOffset * halfRealScreen), logSize);

            // These encompass the entire area covered by quest log UI.
            if (LogArea.Contains(MouseCanvas))
                Main.LocalPlayer.mouseInterface = true;

            #region Canvas Modification

            MoveTab = LogArea.CookieCutter(new(0f, 0f), new(0.059f, 1f));
            if ((MoveTab.Contains(MouseCanvas) || CanvasMoving) && !CanvasResizing)
            {
                // Reset the position if right clicked.
                if (RightMouseJustPressed)
                {
                    QuestLogDrawer.LogPositionOffset = Vector2.Zero;
                    CanvasMoving = false;
                    CachedMouseClick = null;
                    goto PostMoveTab;
                }

                if (!LeftMouseHeld)
                {
                    CanvasMoving = false;
                    CachedMouseClick = null;
                    goto PostMoveTab;
                }

                CanvasMoving = true;

                if (!CachedMouseClick.HasValue)
                    CachedMouseClick = ScaledMousePos;

                else
                {
                    // Move the canvas based on mouse movement and re-size the log area to match.
                    Vector2 mouseMovement = ScaledMousePos - CachedMouseClick.Value;
                    CachedMouseClick = ScaledMousePos;
                    QuestLogDrawer.LogPositionOffset += mouseMovement / halfScreen;

                    LogArea = CenteredRectangle(halfRealScreen + (QuestLogDrawer.LogPositionOffset * halfRealScreen), logSize);
                    MoveTab = LogArea.CookieCutter(new(0f, 0f), new(0.059f, 1f));
                }
            }

        PostMoveTab:

            ResizeTab = LogArea.CookieCutter(new(1f, 1f), new(0.05f, 0.05f));
            if ((ResizeTab.Contains(MouseCanvas) || CanvasResizing) && !CanvasMoving)
            {
                Main.LocalPlayer.mouseInterface = true;

                // Reset the scale on right click.
                if (RightMouseJustPressed)
                {
                    QuestLogDrawer.LogScale = 1f;
                    CanvasResizing = false;
                    goto PostResizeTab;
                }

                if (!LeftMouseHeld)
                {
                    CanvasResizing = false;
                    goto PostResizeTab;
                }

                CanvasResizing = true;

                Vector2 areaLineAngle = LogArea.BottomRight() - LogArea.Center();
                Vector2 mouseLineAngle = areaLineAngle.RotatedBy(-MathHelper.PiOver2);

                Vector2 intersection = GetPointOfIntersection(LogArea.Center(), areaLineAngle, ScaledMousePos, mouseLineAngle);

                if (intersection.X < LogArea.Center().X)
                    goto PostResizeTab;

                Vector2 defaultLogSize = QuestAssets.BasicQuestCanvas.Texture.Size();
                float scale = (intersection - LogArea.Center()).Length() / (defaultLogSize * 0.5f).Length();

                // Minimum scale
                if (scale >= 0.4f)
                    QuestLogDrawer.LogScale = scale;

                LogArea = CenteredRectangle(halfRealScreen + (QuestLogDrawer.LogPositionOffset * halfRealScreen), logSize);
                ResizeTab = LogArea.CookieCutter(new(1f, 1f), new(0.05f, 0.05f));
            }

        PostResizeTab:

            #endregion

            UpdateDesignerToggle();

            Rectangle library = LogArea.CookieCutter(new(-0.505f, -0.07f), new(0.43f, 0.86f));

            Books = library.CookieCutter(new(-0.5f, 0f), new(0.5f, 1f)).CreateMargins(right: 4);
            Chapters = library.CookieCutter(new(0.5f, 0f), new(0.5f, 1f)).CreateMargins(left: 4);

            Canvas = LogArea.CookieCutter(new(0.505f, -0.07f), new(0.43f, 0.86f));
        }

        public override void DrawLog(SpriteBatch spriteBatch)
        {
            Vector2 halfRealScreen = QuestLogDrawer.RealScreenSize * 0.5f;

            // Display "under construction" message while mod is in development!
            if (!DebugDisplay)
            {
                Texture2D construction = QuestAssets.UnderConstruction;
                spriteBatch.Draw(construction, halfRealScreen + (QuestLogDrawer.LogPositionOffset * halfRealScreen), null, Color.White, 0f, construction.Size() / 2f, QuestLogDrawer.LogScale, SpriteEffects.None, 0f);
                return;
            }

            Texture2D logTexture = QuestAssets.BasicQuestCanvas;
            spriteBatch.Draw(logTexture, halfRealScreen + (QuestLogDrawer.LogPositionOffset * halfRealScreen), null, Color.White, 0f, logTexture.Size() / 2f, QuestLogDrawer.LogScale, SpriteEffects.None, 0f);

            if (!QuestLogDrawer.UseDesigner)
                spriteBatch.DrawRectangle(LogArea, Color.Red);

            spriteBatch.DrawRectangle(Books, Color.LimeGreen);
            spriteBatch.DrawRectangle(Chapters, Color.LimeGreen);

            spriteBatch.DrawRectangle(Canvas, Color.Blue);

            spriteBatch.DrawRectangle(MoveTab, Color.Yellow);
            spriteBatch.DrawRectangle(ResizeTab, Color.Orange);

            if (QuestBooks.DesignerEnabled)
                spriteBatch.DrawRectangle(DesignerToggle, Color.Orange);
        }

        #endregion

        #region Designer

        public override void UpdateDesigner()
        {
            UpdateLog();
        }

        public override void DrawDesigner(SpriteBatch spriteBatch)
        {
            DrawLog(spriteBatch);

            if (!QuestLogDrawer.UseDesigner)
                return;

            spriteBatch.DrawRectangle(LogArea, Color.Aquamarine);
        }

        #endregion

        #region Components

        private static void UpdateDesignerToggle()
        {
            if (QuestBooks.DesignerEnabled)
            {
                DesignerToggle = LogArea.CookieCutter(new(0f, -1.1f), new(0.06f, 0.05f));

                if (DesignerToggle.Contains(MouseCanvas))
                {
                    Main.LocalPlayer.mouseInterface = true;

                    if (LeftMouseJustPressed)
                    {
                        QuestLogDrawer.UseDesigner = !QuestLogDrawer.UseDesigner;

                        if (QuestLogDrawer.UseDesigner)
                            SoundEngine.PlaySound(SoundID.Item28);

                        else
                            SoundEngine.PlaySound(SoundID.Item78);
                    }
                }
            }
        }

        #endregion

        #region State Caching

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

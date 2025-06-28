using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultLogStyles
{
    public partial class BasicQuestLogStyle
    {
        // The scale to draw to the render targets
        protected static float TargetScale { get; set; } = 1f;

        private static Vector2? cachedMouseClick = null;
        private static bool canvasMoving = false;
        private static bool canvasResizing = false;

        private static void UpdateDesignerToggle()
        {
            if (QuestBooksMod.DesignerEnabled)
            {
                Rectangle designerToggle = LogArea.CookieCutter(new(0.94f, -1.08f), new(0.05f, 0.06f));
                bool designerHovered = false;

                if (designerToggle.Contains(MouseCanvas))
                {
                    MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ToggleDesigner");
                    LockMouse();
                    designerHovered = true;

                    if (LeftMouseJustPressed)
                    {
                        UseDesigner = !UseDesigner;
                        SelectedElement = null;
                        SoundEngine.PlaySound(UseDesigner ? SoundID.Item28 : SoundID.Item78);
                    }
                }

                DrawTasks.Add(sb =>
                {
                    Texture2D texture = designerHovered ? QuestAssets.DesignerOnButtonHovered : QuestAssets.DesignerOnButton;
                    float scale = designerToggle.Width / (float)texture.Width;
                    sb.Draw(texture, designerToggle.Center(), null, Color.White, 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                });
            }
        }

        private static void UpdateCanvasMovement(Vector2 halfRealScreen, Vector2 logSize)
        {
            Rectangle moveTab = LogArea.CookieCutter(new(0f, 0f), new(0.059f, 1f));
            if ((moveTab.Contains(MouseCanvas) || canvasMoving) && !canvasResizing)
            {
                // Reset the position if right clicked.
                if (RightMouseJustPressed)
                {
                    LogPositionOffset = Vector2.Zero;
                    canvasMoving = false;
                    cachedMouseClick = null;
                    return;
                }

                if (!LeftMouseHeld || (!canvasMoving && !LeftMouseJustPressed))
                {
                    canvasMoving = false;
                    cachedMouseClick = null;
                    return;
                }

                canvasMoving = true;

                if (!cachedMouseClick.HasValue)
                    cachedMouseClick = ScaledMousePos;

                else
                {
                    // Move the canvas based on mouse movement and re-size the log area to match.
                    Vector2 mouseMovement = ScaledMousePos - cachedMouseClick.Value;
                    cachedMouseClick = ScaledMousePos;
                    LogPositionOffset += mouseMovement / halfRealScreen;

                    LogArea = CenteredRectangle(halfRealScreen + (LogPositionOffset * halfRealScreen), logSize);
                }
            }
        }

        private static void UpdateCanvasResizing(Vector2 halfRealScreen)
        {
            Rectangle resizeTab = LogArea.CookieCutter(new(1.01f, 1.02f), new(0.062f, 0.09f));
            if ((resizeTab.Contains(MouseCanvas) || canvasResizing) && !canvasMoving)
            {
                Main.LocalPlayer.mouseInterface = true;

                // Reset the scale on right click.
                if (RightMouseJustPressed)
                {
                    LogScale = 1f;
                    canvasResizing = false;
                    wantsRetarget = true;
                    goto PostResize;
                }

                if (!LeftMouseHeld || (!canvasResizing && !LeftMouseJustPressed))
                {
                    if (canvasResizing)
                        wantsRetarget = true;

                    canvasResizing = false;
                    goto PostResize;
                }

                canvasResizing = true;

                Vector2 areaLineAngle = LogArea.BottomRight() - LogArea.Center();
                Vector2 mouseLineAngle = areaLineAngle.RotatedBy(-MathHelper.PiOver2);

                Vector2 intersection = GetPointOfIntersection(LogArea.Center(), areaLineAngle, ScaledMousePos, mouseLineAngle);

                if (intersection.X < LogArea.Center().X)
                    goto PostResize;

                Vector2 defaultLogSize = QuestAssets.BasicQuestCanvas.Asset.Size();
                float scale = (intersection - LogArea.Center()).Length() / (defaultLogSize * 0.5f).Length();

                // Clamped scale
                if (scale is >= 0.4f and <= 2f)
                    LogScale = scale;

                Vector2 logSize = QuestAssets.BasicQuestCanvas.Asset.Size() * LogScale;
                LogArea = CenteredRectangle(halfRealScreen + (LogPositionOffset * halfRealScreen), logSize);
            }

        PostResize:
            DrawTasks.Add(sb => sb.Draw(QuestAssets.ResizeIndicator, LogArea.BottomRight(), null, Color.White, 0f, QuestAssets.ResizeIndicator.Asset.Size() * 0.5f, LogScale, SpriteEffects.None, 0f));
        }
    }
}

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using QuestBooks.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultQuestLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private static Vector2? CachedMouseClick = null;
        private static bool CanvasMoving = false;
        private static bool CanvasResizing = false;
        private static float targetScale = 1f;

        private static void UpdateDesignerToggle()
        {
            if (QuestBooksMod.DesignerEnabled)
            {
                Rectangle designerToggle = LogArea.CookieCutter(new(0.94f, -1.08f), new(0.05f, 0.06f));

                if (designerToggle.Contains(MouseCanvas))
                {
                    MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.ToggleDesigner");
                    LockMouse();

                    if (LeftMouseJustPressed)
                    {
                        UseDesigner = !UseDesigner;
                        SoundEngine.PlaySound(UseDesigner ? SoundID.Item28 : SoundID.Item78);
                    }
                }

                AddRectangle(designerToggle, Color.Yellow, fill: true);
            }
        }

        private static void UpdateCanvasMovement(Vector2 halfRealScreen, Vector2 logSize)
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
                    wantsRetarget = true;
                    goto PostResize;
                }

                if (!LeftMouseHeld || (!CanvasResizing && !LeftMouseJustPressed))
                {
                    if (CanvasResizing)
                        wantsRetarget = true;

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

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameInput;
using Terraria;

namespace QuestBooks.QuestLog.DefaultQuestLogStyles;

public partial class BasicQuestLogStyle
{
    public static bool LeftMouseJustPressed = false;
    public static bool LeftMouseHeld = false;
    public static bool LeftMouseJustReleased = false;

    public static bool RightMouseJustPressed = false;
    public static bool RightMouseHeld = false;
    public static bool RightMouseJustReleased = false;

    public static string MouseTooltip = "";

    public static void DrawMouseTooltip()
    {
        if (MouseTooltip is not null)
            Main.instance.MouseText(MouseTooltip, "", noOverride: true);
    }

    public static void UpdateMousePosition(Vector2 halfScreen, Vector2 halfRealScreen)
    {
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
    }

    public static void UpdateMouseClicks()
    {
        if (Main.mouseLeft)
        {
            LeftMouseJustPressed = !LeftMouseHeld;
            LeftMouseHeld = true;
        }
        else
        {
            LeftMouseJustReleased = LeftMouseHeld;
            LeftMouseHeld = false;
        }

        if (Main.mouseRight)
        {
            RightMouseJustPressed = !RightMouseHeld;
            RightMouseHeld = true;
        }
        else
        {
            RightMouseJustReleased = RightMouseHeld;
            RightMouseHeld = false;
        }
    }
}

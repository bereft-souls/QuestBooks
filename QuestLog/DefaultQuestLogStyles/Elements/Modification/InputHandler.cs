using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameInput;
using Terraria;
using Terraria.ModLoader.UI;
using Terraria.UI;

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

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        if (MouseTooltip is null)
            return;

        if (string.IsNullOrWhiteSpace(MouseTooltip))
            Main.instance.MouseTextNoOverride(MouseTooltip);

        else
            UICommon.TooltipMouseText(MouseTooltip);
    }

    public static void UpdateMousePosition(Vector2 halfScreen, Vector2 halfRealScreen)
    {
        ScaledMousePos = (Main.MouseScreen - halfScreen) / halfScreen * halfRealScreen + halfRealScreen;
        MouseCanvas = ScaledMousePos.ToPoint();
        MouseTooltip = null;

        // These encompass the entire area covered by quest log UI.
        if (LogArea.Contains(MouseCanvas))
        {
            LockMouse();
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

    public static void LockMouse()
    {
        MouseTooltip ??= "";
        Main.LocalPlayer.mouseInterface = true;
        PlayerInput.LockVanillaMouseScroll("QuestBooks/QuestLog");
    }
}

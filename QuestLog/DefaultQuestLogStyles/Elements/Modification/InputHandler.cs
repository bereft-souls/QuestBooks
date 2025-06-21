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
    protected static bool LeftMouseJustPressed = false;
    protected static bool LeftMouseHeld = false;
    protected static bool LeftMouseJustReleased = false;

    protected static bool RightMouseJustPressed = false;
    protected static bool RightMouseHeld = false;
    protected static bool RightMouseJustReleased = false;

    protected static string MouseTooltip = "";

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        if (MouseTooltip is null)
            return;

        if (string.IsNullOrWhiteSpace(MouseTooltip))
            Main.instance.MouseTextNoOverride(MouseTooltip);

        else
            UICommon.TooltipMouseText(MouseTooltip);
    }

    protected static void UpdateMousePosition(Vector2 halfScreen, Vector2 halfRealScreen)
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

    protected static void UpdateMouseClicks()
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

    protected static void LockMouse()
    {
        MouseTooltip ??= "";
        Main.LocalPlayer.mouseInterface = true;
        PlayerInput.LockVanillaMouseScroll("QuestBooks/QuestLog");
    }
}

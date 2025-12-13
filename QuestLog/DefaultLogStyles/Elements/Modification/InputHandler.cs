using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace QuestBooks.QuestLog.DefaultLogStyles;

public partial class BasicQuestLogStyle
{
    protected bool LeftMouseJustPressed = false;
    protected bool LeftMouseHeld = false;
    protected bool LeftMouseJustReleased = false;

    protected bool RightMouseJustPressed = false;
    protected bool RightMouseHeld = false;
    protected bool RightMouseJustReleased = false;

    protected string MouseTooltip = "";
    protected bool CancelChat = false;
    protected readonly List<Action> ExtraInferfaceLayerMods = [];

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        if (CancelChat)
        {
            Main.drawingPlayerChat = false;
            CancelChat = false;
        }

        if (MouseTooltip is not null)
            Main.HoverItem = new Item();

        foreach (var action in ExtraInferfaceLayerMods)
            action?.Invoke();

        ExtraInferfaceLayerMods.Clear();

        if (MouseTooltip is null || Main.HoverItem.type > 0)
            return;

        if (string.IsNullOrWhiteSpace(MouseTooltip))
            Main.instance.MouseTextNoOverride(MouseTooltip);

        else
            UICommon.TooltipMouseText(MouseTooltip);
    }

    protected void UpdateMousePosition(Vector2 halfScreen, Vector2 halfRealScreen)
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

    protected void UpdateMouseClicks()
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

    protected void LockMouse()
    {
        MouseTooltip ??= "";
        Main.LocalPlayer.mouseInterface = true;
        PlayerInput.LockVanillaMouseScroll("QuestBooks/QuestLog");
    }
}

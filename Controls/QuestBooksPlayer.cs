using QuestBooks.QuestLog.DefaultQuestLogStyles;
using QuestBooks.Systems;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace QuestBooks.Controls
{
    internal class DebugInfoCommand : ModCommand
    {
        public override string Command => "questdebug";

        public override CommandType Type => CommandType.Chat;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            BasicQuestLogStyle.DebugDisplay = !BasicQuestLogStyle.DebugDisplay;

            if (BasicQuestLogStyle.DebugDisplay)
                SoundEngine.PlaySound(SoundID.Item43);

            else
                SoundEngine.PlaySound(SoundID.Item44);
        }
    }

    internal class QuickDesignerCommand : ModCommand
    {
        public override string Command => "questdesigner";

        public override CommandType Type => CommandType.Chat;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            QuestBooks.DesignerEnabled = !QuestBooks.DesignerEnabled;

            if (QuestBooks.DesignerEnabled)
                SoundEngine.PlaySound(SoundID.AchievementComplete);

            else
            {
                QuestLogDrawer.UseDesigner = false;
                SoundEngine.PlaySound(SoundID.Unlock);
            }
        }
    }

    internal class QuestBooksPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (!Keybinds.ToggleQuestLog.JustPressed)
                return;

            // Toggle the QuestLog.
            QuestLogDrawer.Toggle();

            // If the inventory is currently open, close it.
            // We can't guarantee the log won't overlap with the inventory,
            // so the easiest way to reduce mouse overlap conflict is just close it.
            if (QuestLogDrawer.DisplayLog && Main.playerInventory)
                Main.playerInventory = false;
        }

        public override void SetControls()
        {
            // If we aren't currently displaying the log, do nothing.
            if (!QuestLogDrawer.DisplayLog)
                return;

            // If player just attempted to toggle the inventory, and their inventory is
            // not currently open, that means the quest log has been open for at least 1 frame.
            //
            // In this case, we turn simply turn the quest log off and prevent the player
            // from immediately opening their inventory.
            if (Main.LocalPlayer.controlInv)
            {
                QuestLogDrawer.Toggle(false);
                Main.LocalPlayer.releaseInventory = false;
            }

            // If the player is opening the creative menu, we also close the log.
            else if (Main.LocalPlayer.controlCreativeMenu && Main.LocalPlayer.difficulty == PlayerDifficultyID.Creative)
            {
                QuestLogDrawer.Toggle(false);
                Main.LocalPlayer.releaseCreativeMenu = false;
            }
        }

        public override void PostUpdate()
        {
            // Hide the log if the player dies.
            if (Main.LocalPlayer.dead && QuestLogDrawer.DisplayLog)
                QuestLogDrawer.Toggle(false);
        }

        public override void OnEnterWorld()
        {
            // Hide the log on world entry.
            QuestLogDrawer.Toggle(false);
        }

        private const string ScaleKey = "QuestBooksScale";
        private const string OffsetKey = "QuestBooksOffset";

        public override void SaveData(TagCompound tag)
        {
            tag[ScaleKey] = QuestLogDrawer.LogScale;
            tag[OffsetKey] = QuestLogDrawer.LogPositionOffset;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet(ScaleKey, out float scale))
                QuestLogDrawer.LogScale = scale;

            if (tag.TryGet(OffsetKey, out Microsoft.Xna.Framework.Vector2 offset))
                QuestLogDrawer.LogPositionOffset = offset;
        }
    }
}

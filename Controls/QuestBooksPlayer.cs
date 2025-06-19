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
    internal class QuickDesignerCommand : ModCommand
    {
        public override string Command => "questdesigner";

        public override CommandType Type => CommandType.Chat;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            QuestBooksMod.DesignerEnabled = !QuestBooksMod.DesignerEnabled;

            if (QuestBooksMod.DesignerEnabled)
                SoundEngine.PlaySound(SoundID.AchievementComplete);

            else
                SoundEngine.PlaySound(SoundID.Unlock);
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

        public override void SaveData(TagCompound tag) => QuestManager.ActiveStyle.SavePlayerData(tag);
        public override void LoadData(TagCompound tag) => QuestManager.ActiveStyle.LoadPlayerData(tag);
    }
}

using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Commands
{
    internal class ToggleDesigner : QuestBooksCommand
    {
        public override string Command => "toggledesigner";

        public override void Run(CommandCaller caller, string[] args)
        {
            if (QuestBooksMod.DesignerEnabled)
            {
                QuestBooksMod.DesignerEnabled = false;
                SoundEngine.PlaySound(SoundID.Unlock);
            }

            else
            {
                QuestBooksMod.EnableDesigner(Mod);
                SoundEngine.PlaySound(SoundID.Item47);
            }

            caller.Reply(this.GetLocalization("Success").Format(QuestBooksMod.DesignerEnabled));
        }
    }
}

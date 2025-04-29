using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace QuestBooks.Controls
{
    internal class Keybinds : ModSystem
    {
        public static ModKeybind ToggleQuestLog;

        public override void Load()
        {
            ToggleQuestLog = KeybindLoader.RegisterKeybind(Mod, "ToggleQuestLog", Keys.P);
        }

        public override void Unload()
        {
            ToggleQuestLog = null;
        }
    }
}

using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace QuestBooks.Controls
{
    internal class Keybinds : ModSystem
    {
        public static ModKeybind ToggleQuestBook;

        public override void Load()
        {
            ToggleQuestBook = KeybindLoader.RegisterKeybind(Mod, "ToggleQuestBook", Keys.P);
        }

        public override void Unload()
        {
            ToggleQuestBook = null;
        }
    }
}

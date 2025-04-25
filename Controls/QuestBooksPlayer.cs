using Terraria.GameInput;
using Terraria.ModLoader;

namespace QuestBooks.Controls
{
    internal class QuestBooksPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (!Keybinds.ToggleQuestBook.JustPressed)
                return;
        }
    }
}

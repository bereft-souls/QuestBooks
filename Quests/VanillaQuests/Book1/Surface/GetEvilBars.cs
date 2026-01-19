using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
{
    internal class GetEvilBars : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for
                          // 1) Smelting Demonite / Crimtane bars,
                          // 2) Craft any gear with either
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc (self note: this is a "child" of EoC quest)
            title = "";
            contents = "";
            texture = null;
        }
    }
}

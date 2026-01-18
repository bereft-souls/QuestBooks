using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book1.Surface
{
    internal class SurviveSlimeRain : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for killing 100 slimes during slime rain
                         // Read this as: having KS summon by slime rain condition but NOT requiring KS to die
                        // This differs from the ACTUAL achievement which does require KS to be killed aswell
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}

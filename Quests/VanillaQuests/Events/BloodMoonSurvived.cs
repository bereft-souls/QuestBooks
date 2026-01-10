using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Events
{
    public class BloodMoonSurvived : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for Blood Moon "defeat"
        }
        public override bool HasInfoPage => true;
        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        { // TODO desc
            title = "";
            contents = "";
            texture = null;
        }
    }
}

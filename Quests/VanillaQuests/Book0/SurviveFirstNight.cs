using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book0
{
    public class SurviveFirstNight : Quest
    {
        public override bool CheckCompletion()
        {
            return false; // TODO: Check for first night passed, probably a timer? achievement mirror
        }
        public override bool HasInfoPage => true;
        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "You can do it!";
            contents = "Survive your first night in the wild and dangerous world of Terraria!";
            texture = null;
        }
    }
}

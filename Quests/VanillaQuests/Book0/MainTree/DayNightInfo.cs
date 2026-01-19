using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter1
{
    public class DayNightInfo : Quest
    {
        public override bool CheckCompletion()
        {
            return true; // info
        }
        
        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "The Day / Night Cycle and its effects on you";
            contents = string.Empty; // TODO: write this up
            texture = null;
        }
    }
}

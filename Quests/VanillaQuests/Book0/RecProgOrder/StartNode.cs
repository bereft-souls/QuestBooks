using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book0.RecProgOrder
{
    public class StartNode : Quest
    {
        public override bool CheckCompletion()
        {
            return true; // Start
        }
        
        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = " ";      // TODO: Short explanation of this page, idea being all of the quests are blacked out until they are *FOUND* on their original trees.
            contents = " ";  // Effectively, making a slowly revealing boss order as the player gets to play the game, 
            texture = null; // instead of telling them what to do and in what order from the start of the game.
        }
    }
}

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter1
{
    public class GuideInteract : Quest
    {
        public override bool CheckCompletion()
        {
            return true; // Should be a check for interacting with the guide (right clicking him)
        }
        public override bool HasInfoPage => true;
        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "The Guide";
            contents = $"A trusty guide to help you along the way with their incredible crafting knowledge and varying useful tips.\n" +
                $"If you ever find an item labeled as a \"Material\", be sure to check it up with him!";
            texture = null;
        }
    }
}

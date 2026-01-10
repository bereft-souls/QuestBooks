using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book0
{
    public class WorldEventsInfo : Quest
    {
        public override bool CheckCompletion()
        {
            return true; // Info
        }
        public override bool HasInfoPage => true;
        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "Events and occurances throughout Terraria";
            contents = null; // TODO: Write this up, this probably shouldnt be JUST combat events (goblins, pirates..) but also smaller things / single time things
                            // like star showers, ladybugs, meteor crashes etc. should be written generically though (dont mention Goblin Invasion or Meteor by name)
            texture = null;
        }
    }
}

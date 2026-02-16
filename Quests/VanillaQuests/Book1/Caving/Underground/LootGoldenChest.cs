using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Quests.VanillaQuests.Book1.Caving.Underground
{
    internal class LootGoldenChest : Quest
    {
        public override bool CheckCompletion()
        {
            throw new NotImplementedException();
            // loot a golden chest and..
            /* Get any 1 of: (theres 3 loot tables depending on depth, hence the gaps)
                    # Mace
                    # Hermes Boots
                    # Band of Regeneration
                    # Magic Mirror
                    # Cloud in a Bottle
                    # Shoe Spikes

                    # Flare Gun
                    # Extractinator

                    # Lava Charm
            */
        }
    }
}

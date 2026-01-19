using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter1
{
    internal class ClassInfo : Quest
    {
        public override bool CheckCompletion()
        {
            return true; // Info node
        }

        public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = "Damage types (or, Classes)";
            contents = "In Terraria, all weapons are categorized into archetypes. That is, each weapon deals a specific type of damage. \r\n" +
                "Several equipments and armors in the game may only benefit 1 archetype, denoted by the class' name." + // TODO: The armor / accessory thing is a bit
                "The 4 different archetypes are:\r\n\r\n" +                                                            // Awkward here, probably rewrite it to the
                "- The Melee -\r\n" +                                                                                 // bottom later.
                "Often utilizing a variety of close combat or short ranged weapons such as flails and boomerangs.\r\n" +
                "A tough and defensive Class.\r\n\r\n" +
                "- The Ranger -\r\n" +
                "Slinging guns, Drawing bows, blasting rockets and more. A versatile archetype that requires the use of Ammo which can grant their weaponry an extra edge.\r\n" +
                "A well rounded Class\r\n\r\n" +
                "- The Mage -\r\n" +
                "Conjuring spells and casting magic with a variety of arcane staves, books and more. Weapons require the use of your Mana.\r\n" +
                "Usually has lower defense for higher damage output\r\n\r\n" +
                "- The Summoner -\r\n" +
                "Commanding an army of minions with a tight grip using supportive whips to fight your battles, allowing you to focus on surviving.\r\n" +
                "A Fragile yet powerful Class.\r\n\r\n" +
                "With that said, the differences between classes arent set in the sand; Mages may still get to use magical guns while Melee can acquire magical swords. \r\n" +
                "So dont worry too much about locking into 1 weapon style, and you can always freely switch between classes at any time!";
            texture = null;
        }
    }
}

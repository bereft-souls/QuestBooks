using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class GetEvilBarArmor : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => EvilBarArmorItemCheck.Complete;

    // TODO: Possibly consider implementing compatibility support for mods that add armor sets made from evil bars, or alternatively their own evil bars.
    public class EvilBarArmorItemCheck : GlobalItem
    {
        /// <summary>
        ///     Gets a value indicating whether the player has crafted the helmet piece of an armor set made
        ///     from an evil bar.
        /// </summary>
        public static bool Head { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the player has crafted the chestplate piece of an armor set
        ///     made from an evil bar.
        /// </summary>
        public static bool Body { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the player has crafted the legging piece of an armor set made
        ///     from an evil bar.
        /// </summary>
        public static bool Legs { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the player has crafted all three pieces of an armor set made
        ///     from an evil bar.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if the player has crafted all three pieces of an armor set made from an
        ///     evil bar; otherwise, <see langword="false"/>.
        /// </value>
        public static bool Complete => Head && Body && Legs;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
            {
                return;
            }

            // TODO: Do we want to enforce that armor pieces must be of the same type (i.e. all Shadow or all Crimson)?
            Head |= item.type == ItemID.CrimsonHelmet || item.type == ItemID.ShadowHelmet;
            Body |= item.type == ItemID.CrimsonScalemail || item.type == ItemID.ShadowScalemail;
            Legs |= item.type == ItemID.CrimsonGreaves || item.type == ItemID.ShadowGreaves;
        }
    }
}
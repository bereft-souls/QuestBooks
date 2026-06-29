using QuestBooks.Systems;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class CraftEvilBarArmor : VanillaQuest
{
    private const string HeadTag = "CraftedEvilArmorHead";

    private const string BodyTag = "CraftedEvilArmorBody";

    private const string LegsTag = "CraftedEvilArmorLegs";

    /// <summary>
    ///     Gets a value indicating whether the player has crafted the helmet piece of an armor set made
    ///     from an evil bar.
    /// </summary>
    public bool Head { get; private set; }

    /// <summary>
    ///     Gets a value indicating whether the player has crafted the chestplate piece of an armor set
    ///     made from an evil bar.
    /// </summary>
    public bool Body { get; private set; }

    /// <summary>
    ///     Gets a value indicating whether the player has crafted the legging piece of an armor set made
    ///     from an evil bar.
    /// </summary>
    public bool Legs { get; private set; }

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Head && Body && Legs;

    public override void SaveProgress(TagCompound tag)
    {
        tag[HeadTag] = Head;
        tag[BodyTag] = Body;
        tag[LegsTag] = Legs;
    }

    public override void LoadProgress(TagCompound tag)
    {
        Head = tag.GetBool(HeadTag);
        Body = tag.GetBool(BodyTag);
        Legs = tag.GetBool(LegsTag);
    }

    public class CraftEvilBarArmorCheck : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
            {
                return;
            }

            var quest = QuestManager.GetQuest<CraftEvilBarArmor>();

            if (!quest.Head)
            {
                quest.Head |= item.type == ItemID.CrimsonHelmet || item.type == ItemID.ShadowHelmet;
            }

            if (!quest.Body)
            {
                quest.Body |= item.type == ItemID.CrimsonScalemail || item.type == ItemID.ShadowScalemail;
            }

            if (!quest.Legs)
            {
                quest.Legs |= item.type == ItemID.CrimsonGreaves || item.type == ItemID.ShadowGreaves;
            }
        }
    }
}
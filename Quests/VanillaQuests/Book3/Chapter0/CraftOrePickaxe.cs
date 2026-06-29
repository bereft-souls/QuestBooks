using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

[ReinitializeDuringResizeArrays]
public class CraftOrePickaxe : VanillaQuest
{
    static CraftOrePickaxe()
    {
        OrePickaxes = ItemID.Sets.Factory.CreateNamedSet("OrePickaxes")
            .Description("Pickaxes that are crafted from ores")
            .RegisterBoolSet(
                ItemID.TinPickaxe,
                ItemID.IronPickaxe,
                ItemID.LeadPickaxe,
                ItemID.SilverPickaxe,
                ItemID.TungstenPickaxe,
                ItemID.GoldPickaxe,
                ItemID.PlatinumPickaxe
            );
    }

    public static readonly bool[] OrePickaxes;

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftOrePickaxeCheck() : CraftItemHook<CraftOrePickaxe>(OrePickaxes);
}
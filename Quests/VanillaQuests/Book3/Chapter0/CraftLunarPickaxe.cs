using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class CraftLunarPickaxe : QBQuest
{
    public static readonly bool[] LunarPickaxes = ItemID.Sets.Factory.CreateNamedSet("LunarPickaxes")
        .Description("Pickaxes that are (intended to be) lunar exclusive")
        .RegisterBoolSet(
            ItemID.SolarFlarePickaxe,
            ItemID.VortexPickaxe,
            ItemID.NebulaPickaxe,
            ItemID.StardustPickaxe,
            ItemID.SolarFlareDrill,
            ItemID.VortexDrill,
            ItemID.NebulaDrill,
            ItemID.StardustDrill
        );

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftLunarPickaxeCheck() : CraftItemHook<CraftLunarPickaxe>(LunarPickaxes);
}
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

[ReinitializeDuringResizeArrays]
public class CraftLunarPickaxe : QBQuest
{
    static CraftLunarPickaxe()
    {
        LunarPickaxes = ItemID.Sets.Factory.CreateNamedSet("LunarPickaxes")
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
    }

    public static readonly bool[] LunarPickaxes;

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftLunarPickaxeCheck() : CraftItemHook<CraftLunarPickaxe>(LunarPickaxes);
}
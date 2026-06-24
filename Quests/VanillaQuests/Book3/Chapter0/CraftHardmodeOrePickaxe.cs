using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

[ReinitializeDuringResizeArrays]
public class CraftHardmodeOrePickaxes : QBQuest
{
    static CraftHardmodeOrePickaxes()
    {
        HardmodePickaxes = ItemID.Sets.Factory.CreateNamedSet("HardmodePickaxes")
            .Description("Pickaxes that are (intended to be) hardmode exclusive")
            .RegisterBoolSet(
                ItemID.CobaltPickaxe,
                ItemID.PalladiumPickaxe,
                ItemID.MythrilPickaxe,
                ItemID.OrichalcumPickaxe,
                ItemID.AdamantitePickaxe,
                ItemID.TitaniumPickaxe
            );
    }

    public static readonly bool[] HardmodePickaxes;

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftHardmodeOrePickaxesCheck() : CraftItemHook<CraftHardmodeOrePickaxes>(item => HardmodePickaxes[item.type]);
}
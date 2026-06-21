namespace QuestBooks.Content.Sets;

public sealed partial class ItemSetsSystem
{
    public static class Pickaxes
    {
        public static bool[] Hardmode { get; internal set; }
    }

    private static void SetupPickaxes()
    {
        Pickaxes.Hardmode = ItemID.Sets.Factory.CreateBoolSet
        (
            ItemID.CobaltPickaxe,
            ItemID.PalladiumPickaxe,
            ItemID.MythrilPickaxe,
            ItemID.OrichalcumPickaxe,
            ItemID.AdamantitePickaxe,
            ItemID.TitaniumPickaxe
        );
    }
}
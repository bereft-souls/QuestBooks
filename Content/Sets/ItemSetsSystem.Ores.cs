namespace QuestBooks.Content.Sets;

public sealed partial class ItemSetsSystem
{
    public static class Ores
    {
        public static bool[] Gem { get; internal set; }
    }

    private static void SetupOres()
    {
        Ores.Gem = ItemID.Sets.Factory.CreateBoolSet
        (
            ItemID.Diamond,
            ItemID.Amber,
            ItemID.Ruby,
            ItemID.Emerald,
            ItemID.Sapphire,
            ItemID.Topaz,
            ItemID.Amethyst
        );
    }
}
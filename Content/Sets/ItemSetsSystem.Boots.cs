namespace QuestBooks.Content.Sets;

public sealed partial class ItemSetsSystem
{
    public static class Boots
    {
        public static bool[] Running { get; internal set; }
    }

    private static void SetupBoots()
    {
        Boots.Running = ItemID.Sets.Factory.CreateBoolSet
        (
            ItemID.SpectreBoots,
            ItemID.FrostsparkBoots,
            ItemID.SailfishBoots,
            ItemID.LavaWaders,
            ItemID.HermesBoots,
            ItemID.FlurryBoots,
            ItemID.LightningBoots,
            ItemID.TerrasparkBoots
        );
    }
}
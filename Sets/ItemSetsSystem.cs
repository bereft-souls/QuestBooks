namespace QuestBooks.Sets;

public sealed class ItemSetsSystem : ModSystem
{
    public static class Boots
    {
        public static bool[] Running { get; internal set; }
    }
    
    public static bool[] Gem { get; private set; }

    public override void PostSetupContent()
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
        
        Gem = ItemID.Sets.Factory.CreateBoolSet
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
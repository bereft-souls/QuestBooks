namespace QuestBooks.Content.Sets;

public sealed partial class ItemSetsSystem
{
    public static class Furniture
    {
        public static bool[] Campfire { get; internal set; }

        public static bool[] Sink { get; internal set; }
    }

    private static void SetupFurniture()
    {
        Furniture.Campfire = ItemID.Sets.Factory.CreateBoolSet
        (
            ItemID.Campfire,
            ItemID.BoneCampfire,
            ItemID.CoralCampfire,
            ItemID.CorruptCampfire,
            ItemID.CrimsonCampfire,
            ItemID.CursedCampfire,
            ItemID.DemonCampfire,
            ItemID.DesertCampfire,
            ItemID.FrozenCampfire,
            ItemID.HallowedCampfire,
            ItemID.IchorCampfire,
            ItemID.JungleCampfire,
            ItemID.MushroomCampfire,
            ItemID.RainbowCampfire,
            ItemID.ShimmerCampfire,
            ItemID.UltraBrightCampfire
        );

        Furniture.Sink = ItemID.Sets.Factory.CreateBoolSet
        (
            ItemID.WoodenSink,
            ItemID.EbonwoodSink,
            ItemID.RichMahoganySink,
            ItemID.PearlwoodSink,
            ItemID.ShadewoodSink,
            ItemID.BorealWoodSink,
            ItemID.PalmWoodSink,
            ItemID.AshWoodSink,
            ItemID.CactusSink,
            ItemID.BambooSink,
            ItemID.DynastySink,
            ItemID.LivingWoodSink,
            ItemID.SkywareSink,
            ItemID.MarbleSink,
            ItemID.GraniteSink,
            ItemID.MeteoriteSink,
            ItemID.ObsidianSink,
            ItemID.BoneSink,
            ItemID.FleshSink,
            ItemID.PumpkinSink,
            ItemID.HoneySink,
            ItemID.LihzahrdSink,
            ItemID.MartianSink,
            ItemID.GlassSink,
            ItemID.SpookySink
        );
    }
}
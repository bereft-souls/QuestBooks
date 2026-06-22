namespace QuestBooks.Content.Sets;

public static class ItemSets
{
    /// <summary>
    ///     Contains sets of items that are categorized as furniture.
    /// </summary>
    public static class Furniture
    {
        /// <summary>
        ///     A set containing all trophies in the game.
        /// </summary>
        public static readonly ContentSet Trophies = new()
        {
            ItemID.KingSlimeTrophy,
            ItemID.EyeofCthulhuTrophy,
            ItemID.EaterofWorldsTrophy,
            ItemID.BrainofCthulhuTrophy,
            ItemID.QueenBeeTrophy,
            ItemID.SkeletronTrophy,
            ItemID.DeerclopsTrophy,
            ItemID.WallofFleshTrophy,
            ItemID.QueenSlimeTrophy,
            ItemID.RetinazerTrophy,
            ItemID.SpazmatismTrophy,
            ItemID.DestroyerTrophy,
            ItemID.SkeletronPrimeTrophy,
            ItemID.PlanteraTrophy,
            ItemID.GolemTrophy,
            ItemID.DukeFishronTrophy,
            ItemID.AncientCultistTrophy,
            ItemID.BetsyMasterTrophy,
            ItemID.MoonLordTrophy,
            ItemID.OgreMasterTrophy,
            ItemID.FlyingDutchmanTrophy,
            ItemID.MourningWoodTrophy,
            ItemID.PumpkingTrophy,
            ItemID.EverscreamTrophy,
            ItemID.SantaNK1Trophy,
            ItemID.IceQueenTrophy,
            ItemID.MartianSaucerTrophy
        };

        /// <summary>
        ///     A set containing all sinks in the game.
        /// </summary>
        public static readonly ContentSet Sinks = new()
        {
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
        };

        /// <summary>
        ///     A set containing all campfires in the game.
        /// </summary>
        public static readonly ContentSet Campfires = new()
        {
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
        };
    }

    /// <summary>
    ///     Contains sets of items that are categorized as accessories.
    /// </summary>
    public static class Accessories
    {
        /// <summary>
        ///     A set containing all running boots in the game.
        /// </summary>
        public static readonly ContentSet Boots = new()
        {
            ItemID.SpectreBoots,
            ItemID.FrostsparkBoots,
            ItemID.SailfishBoots,
            ItemID.LavaWaders,
            ItemID.HermesBoots,
            ItemID.FlurryBoots,
            ItemID.LightningBoots,
            ItemID.TerrasparkBoots
        };
    }

    /// <summary>
    ///     Contains sets of items that are categorized as materials.
    /// </summary>
    public static class Materials
    {
        /// <summary>
        ///     A set containing all gems in the game.
        /// </summary>
        public static readonly ContentSet Gems = new()
        {
            ItemID.Diamond,
            ItemID.Amber,
            ItemID.Ruby,
            ItemID.Emerald,
            ItemID.Sapphire,
            ItemID.Topaz,
            ItemID.Amethyst
        };
    }

    /// <summary>
    ///     Contains sets of items that are categorized as tools.
    /// </summary>
    public static class Tools
    {
        /// <summary>
        ///     A set containing all hardmode pickaxes in the game.
        /// </summary>
        public static class Pickaxes
        {
            /// <summary>
            ///     A set containing all hardmode pickaxes in the game.
            /// </summary>
            public static readonly ContentSet Hardmode = new()
            {
                ItemID.CobaltPickaxe,
                ItemID.PalladiumPickaxe,
                ItemID.MythrilPickaxe,
                ItemID.OrichalcumPickaxe,
                ItemID.AdamantitePickaxe,
                ItemID.TitaniumPickaxe
            };
        }
    }
}
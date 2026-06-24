using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftSink : QBQuest
{
    public static readonly bool[] Sinks = ItemID.Sets.Factory.CreateNamedSet("Sinks")
        .Description("All sink furniture items")
        .RegisterBoolSet(
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

    public override bool CheckCompletion() => false;

    public class CraftSinkCheck() : CraftItemHook<CraftSink>(Sinks);
}
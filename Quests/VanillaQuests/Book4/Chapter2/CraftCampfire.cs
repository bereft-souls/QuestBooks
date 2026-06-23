using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter2;

public class CraftCampfire : QBQuest
{
    public static readonly bool[] Campfires = NPCID.Sets.Factory.CreateNamedSet("Campfires")
        .Description("All campfires")
        .RegisterBoolSet(
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

    public override bool CheckCompletion() => false;

    public class CraftCampfireCheck() : CraftItemCheck<CraftCampfire>(item => Campfires[item.type]);
}
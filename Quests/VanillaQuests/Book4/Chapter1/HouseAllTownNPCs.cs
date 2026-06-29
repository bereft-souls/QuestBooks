namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseAllTownNPCs : VanillaQuest
{
    public override bool CheckCompletion() => AllNPCs(
        NPCID.Guide,
        NPCID.Merchant,
        NPCID.Nurse,
        NPCID.Demolitionist,
        NPCID.DyeTrader,
        NPCID.Angler,
        NPCID.BestiaryGirl,
        NPCID.Dryad,
        NPCID.Painter,
        NPCID.Golfer,
        NPCID.ArmsDealer,
        NPCID.DD2Bartender,
        NPCID.Stylist,
        NPCID.GoblinTinkerer,
        NPCID.WitchDoctor,
        NPCID.Clothier,
        NPCID.Mechanic,
        NPCID.PartyGirl,
        NPCID.Wizard,
        NPCID.TaxCollector,
        NPCID.Truffle,
        NPCID.Pirate,
        NPCID.Steampunker,
        NPCID.Cyborg,
        NPCID.Princess
    );
}
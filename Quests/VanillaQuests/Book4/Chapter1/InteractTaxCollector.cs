using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class InteractTaxCollector : VanillaQuest
{
    /// <summary>
    ///     The amount of gold coins the player must have accumulated from taxes in order to complete the
    ///     quest.
    /// </summary>
    public static readonly int Coins = Item.buyPrice(0, 25);

    public override QuestType QuestType => QuestType.Player;

    public override void Load() => On_Player.CollectTaxes += Check;

    public override bool CheckCompletion() => false;

    private static void Check(On_Player.orig_CollectTaxes orig, Player self)
    {
        orig(self);

        if (self.taxMoney < Coins)
            return;

        QuestBooksMod.MarkComplete<InteractTaxCollector>();
    }
}
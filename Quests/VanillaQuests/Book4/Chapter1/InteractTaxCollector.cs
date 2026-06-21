using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class InteractTaxCollector : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class InteractTaxCollectorCheck : ModSystem
    {
        /// <summary>
        ///     The amount of gold coins the player must have accumulated from taxes in order to complete the quest.
        /// </summary>
        public static readonly int Coins = Item.buyPrice(0, 25);
        
        public override void Load() => On_Player.CollectTaxes += Check;

        private static void Check(On_Player.orig_CollectTaxes orig, Player self)
        {
            orig(self);
            
            if (self.taxMoney < Coins)
                return;
            
            QuestManager.MarkComplete<InteractTaxCollector>();
        }
    }
}
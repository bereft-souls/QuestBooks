using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class InteractTravellingMerchant : QBQuest
{
    public override bool CheckCompletion() => false;

    public class InteractTravellingMerchantCheck : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not BuyItemCreationContext buy || buy.VendorNPC.type != NPCID.TravellingMerchant)
                return;

            QuestManager.MarkComplete<InteractTravellingMerchant>();
        }
    }
}
using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class BuySafe : QBQuest
{
    public override bool CheckCompletion() => false;

    public class BuySafeCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Safe;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not BuyItemCreationContext)
                return;

            QuestManager.MarkComplete<BuySafe>();
        }
    }
}
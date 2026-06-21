using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftAnvil : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftAnvilCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.IronAnvil || entity.type == ItemID.LeadAnvil;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
                return;

            QuestManager.MarkComplete<CraftAnvil>();
        }
    }
}
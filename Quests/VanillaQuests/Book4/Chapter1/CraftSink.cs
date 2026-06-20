using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class CraftSink : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftSinkCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.WoodenSink;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
                return;

            QuestManager.MarkComplete<CraftSink>();
        }
    }
}
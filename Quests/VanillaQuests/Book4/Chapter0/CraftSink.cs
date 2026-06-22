using QuestBooks.Content.Sets;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftSink : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftSinkCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => ItemSets.Furniture.Sinks[entity.type];

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
                return;

            QuestManager.MarkComplete<CraftSink>();
        }
    }
}
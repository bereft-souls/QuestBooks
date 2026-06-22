using QuestBooks.Content.Sets;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter2;

public class CraftCampfire : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftCampfireCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => ItemSets.Furniture.Campfires[entity.type];

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
                return;

            QuestManager.MarkComplete<CraftCampfire>();
        }
    }
}
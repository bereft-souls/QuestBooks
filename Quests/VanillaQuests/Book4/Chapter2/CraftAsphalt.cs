using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter2;

public class CraftAsphalt : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftAsphaltCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == -1;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext recipe || recipe.DestinationStack.stack < 100)
                return;

            QuestManager.MarkComplete<CraftAsphalt>();
        }
    }
}
using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class CraftHallowedPick : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftHallowedPickCheck() : CraftItemHook(Complete)
    {
        private static void Complete(Item item, RecipeItemCreationContext context)
        {
            if (item.pick < 0 || !context.Recipe.HasIngredient(ItemID.HallowedBar))
                return;

            QuestManager.MarkComplete<CraftHallowedPick>();
        }
    }
}
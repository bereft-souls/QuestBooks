using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class CraftHallowedPick : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftHallowedPickCheck() : CraftItemHook(OnCraft)
    {
        private static void OnCraft(Item item, RecipeItemCreationContext context)
        {
            if (!context.Recipe.HasIngredient(ItemID.HallowedBar))
                return;

            QuestManager.MarkComplete<CraftHallowedPick>();
        }
    }
}
using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter2;

public class UseStardustFragments : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class UseStardustFragmentsCheck() : CraftItemHook(OnCraft)
    {
        private static void OnCraft(Item item, RecipeItemCreationContext context)
        {
            if (!context.Recipe.HasTile(TileID.LunarCraftingStation) || !context.Recipe.HasIngredient(ItemID.FragmentStardust))
                return;

            QuestManager.MarkComplete<UseStardustFragments>();
        }
    }
}
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter2;

public class UseVortexFragments : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class UseVortexFragmentsCheck : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext recipe || !recipe.Recipe.HasTile(TileID.LunarCraftingStation) || !recipe.Recipe.HasIngredient(ItemID.FragmentVortex))
                return;

            QuestManager.MarkComplete<UseVortexFragments>();
        }
    }
}
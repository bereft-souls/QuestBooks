using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter2;

public class UseSolarFragments : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class UseSolarFragmentsCheck : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext recipe || !recipe.Recipe.HasTile(TileID.LunarCraftingStation) || !recipe.Recipe.HasIngredient(ItemID.FragmentSolar))
                return;

            QuestManager.MarkComplete<UseSolarFragments>();
        }
    }
}
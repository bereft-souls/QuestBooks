using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class CraftHellstoneGear : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class HellstoneGearItemCheck : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext recipe || !recipe.Recipe.HasIngredient(ItemID.HellstoneBar))
                return;

            QuestManager.CompleteQuest<CraftHellstoneGear>();
        }
    }
}
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class CraftHallowedGear : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class HallowedGearItemCheck : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext recipe || !recipe.Recipe.HasIngredient(ItemID.HallowedBar))
                return;

            QuestManager.CompleteQuest<CraftHallowedGear>();
        }
    }
}
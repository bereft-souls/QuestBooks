using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class CraftMeteorGear : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class MeteorGearItemCheck : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext recipe || !recipe.Recipe.HasIngredient(ItemID.MeteoriteBar))
                return;

            QuestManager.CompleteQuest<CraftCactusGear>();
        }
    }
}
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class ShimmerTransmute : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class ShimmerTransmuteCheck : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext recipe || recipe.Recipe.DecraftDisabled)
                return;

            QuestManager.MarkComplete<ShimmerTransmute>();
        }
    }
}
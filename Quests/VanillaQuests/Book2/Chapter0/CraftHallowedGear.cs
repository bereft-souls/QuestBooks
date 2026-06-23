using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class CraftHallowedGear : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class HallowedGearItemCheck() : CraftItemHook(Complete)
    {
        private static void Complete(Item item, RecipeItemCreationContext context)
        {
            if (context.Recipe.HasIngredient(ItemID.HallowedBar))
            {
                return;
            }
            
            QuestManager.CompleteQuest<CraftHallowedGear>();
        }
    }
}
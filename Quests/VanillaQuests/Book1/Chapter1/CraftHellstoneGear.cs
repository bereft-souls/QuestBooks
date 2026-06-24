using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class CraftHellstoneGear : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class HellstoneGearItemCheck() : CraftItemHook(Complete)
    {
        private static void Complete(Item item, RecipeItemCreationContext context)
        {
            if (!context.Recipe.HasIngredient(ItemID.HellstoneBar))
            {
                return;
            }

            QuestManager.MarkComplete<CraftHellstoneGear>();
        }
    }
}
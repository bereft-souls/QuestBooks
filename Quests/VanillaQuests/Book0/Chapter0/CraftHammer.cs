using QuestBooks.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class CraftHammer : QBQuest
{
    public override bool CheckCompletion() => false;

    public class ItemCheck : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext || item.hammer <= 0)
                return;

            QuestManager.CompleteQuest<CraftHammer>();
        }
    }
}

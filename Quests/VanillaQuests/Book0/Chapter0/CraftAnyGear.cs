using QuestBooks.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class CraftAnyGear : QBQuest
{
    public override bool CheckCompletion() => false;

    public class ItemCheck : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
                return;

            if (item.pick > 0 || item.hammer > 0 || item.axe > 0)
                return;

            if (item.defense == 0 && item.damage <= 0)
                return;

            QuestManager.CompleteQuest<CraftAnyGear>();
        }
    }
}

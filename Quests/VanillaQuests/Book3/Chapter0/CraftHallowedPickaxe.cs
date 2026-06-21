using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class CraftHallowedPickaxe : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftHallowedPickaxeCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Drax || entity.type == ItemID.PickaxeAxe;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
                return;

            QuestManager.MarkComplete<CraftHallowedPickaxe>();
        }
    }
}
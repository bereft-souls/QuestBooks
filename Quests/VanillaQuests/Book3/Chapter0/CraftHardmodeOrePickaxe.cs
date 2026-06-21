using QuestBooks.Content.Sets;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class CraftHardmodeOrePickaxes : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftHardmodeOrePickaxesCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => ItemSetsSystem.Pickaxes.Hardmode[entity.type];

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
                return;

            QuestManager.MarkComplete<CraftHardmodeOrePickaxes>();
        }
    }
}
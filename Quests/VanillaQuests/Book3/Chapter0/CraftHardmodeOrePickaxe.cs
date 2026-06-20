using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class CraftHardmodeOrePickaxes : QBQuest
{
    public override QuestType QuestType => QuestType.Player;
    
    public override bool CheckCompletion() => false;

    // TODO: Implementation.
    public class CraftHardmodeOrePickaxesCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == -1;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
                return;

            QuestManager.MarkComplete<CraftHardmodeOrePickaxes>();
        }
    }
}
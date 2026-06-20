using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class CraftShroomiteDiggingClaws : QBQuest
{
    public override QuestType QuestType => QuestType.Player;
    
    public override bool CheckCompletion() => false;

    public class CraftShroomiteDiggingClawsCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.ShroomiteDiggingClaw;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
                return;

            QuestManager.MarkComplete<CraftShroomiteDiggingClaws>();
        }
    }
}
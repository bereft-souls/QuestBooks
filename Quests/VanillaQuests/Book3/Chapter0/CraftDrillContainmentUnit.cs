using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class CraftDrillContainmentUnit : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftDrillContainmentUnitCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.DrillContainmentUnit;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
                return;

            QuestManager.MarkComplete<CraftDrillContainmentUnit>();
        }
    }
}
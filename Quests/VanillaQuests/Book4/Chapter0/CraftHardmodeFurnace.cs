using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftHardmodeFurnace : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftHardmodeFurnaceCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.AdamantiteForge || entity.type == ItemID.TitaniumForge;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
                return;

            QuestManager.MarkComplete<CraftHardmodeFurnace>();
        }
    }
}
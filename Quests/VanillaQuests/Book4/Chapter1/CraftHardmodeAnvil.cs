using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class CraftHardmodeAnvil : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftHardmodeAnvilCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.MythrilAnvil || entity.type == ItemID.MythrilAnvil;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
                return;

            QuestManager.MarkComplete<CraftHardmodeAnvil>();
        }
    }
}
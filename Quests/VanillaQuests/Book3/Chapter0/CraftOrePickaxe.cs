using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class CraftOrePickaxe : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftOrePickaxeCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.TinPickaxe
            || entity.type == ItemID.IronPickaxe
            || entity.type == ItemID.LeadPickaxe
            || entity.type == ItemID.SilverPickaxe
            || entity.type == ItemID.TungstenPickaxe
            || entity.type == ItemID.GoldPickaxe
            || entity.type == ItemID.PlatinumPickaxe;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
                return;

            QuestManager.MarkComplete<CraftOrePickaxe>();
        }
    }
}
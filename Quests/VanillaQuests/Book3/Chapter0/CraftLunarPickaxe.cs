using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class CraftLunarPickaxe : QBQuest
{
    public override QuestType QuestType => QuestType.Player;
    
    public override bool CheckCompletion() => false;

    public class CraftLunarPickaxeCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.SolarFlarePickaxe 
            || entity.type == ItemID.SolarFlareDrill
            || entity.type == ItemID.VortexPickaxe 
            || entity.type == ItemID.VortexDrill
            || entity.type == ItemID.NebulaPickaxe
            || entity.type == ItemID.NebulaDrill
            || entity.type == ItemID.StardustPickaxe
            || entity.type == ItemID.StardustDrill;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
                return;

            QuestManager.MarkComplete<CraftLunarPickaxe>();
        }
    }
}
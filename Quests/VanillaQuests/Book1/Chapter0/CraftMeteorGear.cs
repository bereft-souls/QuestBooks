using System.Linq;
using QuestBooks.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class CraftMeteorGear : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion()
    {
        return false;
    }

    public class MeteorGearItemCheck : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext recipe)
            {
                return;
            }

            // Ensures automatic compatibility with mods that add new gear using meteorite bars as an ingredient, as opposed to checking for specific item types.
            var ingredients = recipe.ConsumedItems;

            if (!ingredients.Any(static item => item.type == ItemID.MeteoriteBar))
            {
                return;
            }

            // TODO: Should gear be considered only as equipments, or should it also include tools and weapons?
            var gear = item.pick <= 0 && item.hammer <= 0 && item.axe <= 0 && (item.defense > 0 || item.damage > 0);

            if (!gear)
            {
                return;
            }
            
            QuestManager.CompleteQuest<CraftCactusGear>();
        }
    }
}
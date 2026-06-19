using QuestBooks.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class CraftAtEvilAltar : QBQuest
{
    public override bool CheckCompletion() => false;
    
    public class EvilAltarItemCheck : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext recipe || !recipe.Recipe.HasTile(TileID.DemonAltar))
            {
                return;
            }

            QuestManager.MarkComplete<CraftAtEvilAltar>();
        }
    }
}
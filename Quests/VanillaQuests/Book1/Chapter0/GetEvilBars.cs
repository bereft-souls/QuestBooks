using QuestBooks.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class GetEvilBars : QBQuest
{
    public override QuestType QuestType => QuestType.Player;
    
    public override bool CheckCompletion() => false;

    public class EvilBarItemCheck : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext recipe || (!recipe.Recipe.HasIngredient(ItemID.CrimtaneBar) && !recipe.Recipe.HasIngredient(ItemID.DemoniteBar)))
            {
                return;
            }
            
            QuestManager.MarkComplete<GetEvilBars>();
        }
    }
}
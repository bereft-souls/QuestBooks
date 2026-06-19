using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class GetEvilBarArmor : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class EvilBarArmorItemCheck : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
            {
                return;
            }
            
            
        }
    }
}
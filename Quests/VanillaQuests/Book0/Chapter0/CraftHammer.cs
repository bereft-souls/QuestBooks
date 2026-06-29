using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class CraftHammer : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class HammerItemCheck : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext || item.hammer <= 0)
                return;

            QuestBooksMod.CompleteQuest<CraftHammer>();
        }
    }
}

using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class CraftChlorophyteExtractinator : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftChlorophyteExtractinatorCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.ChlorophyteExtractinator;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
                return;

            QuestManager.MarkComplete<CraftChlorophyteExtractinator>();
        }
    }
}
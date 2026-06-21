using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class CraftSpectreBars : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftSpectreBarsCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.SpectreBar;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext)
                return;

            QuestManager.MarkComplete<CraftSpectreBars>();
        }
    }
}
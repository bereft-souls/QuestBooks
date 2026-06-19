using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class GetSparta : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class SpartaItemCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Gladius;

        public override bool? UseItem(Item item, Player player)
        {
            if (player.HasArmorSet(ItemID.GladiatorHelmet, ItemID.GladiatorBreastplate, ItemID.GladiatorLeggings))
                QuestManager.MarkComplete<GetSparta>();

            return true;
        }
    }
}
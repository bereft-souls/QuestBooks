using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class GetNinjaSet : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class NinjaSetItemCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Shuriken;

        public override bool? UseItem(Item item, Player player)
        {
            if (player.HasArmorSet(ItemID.NinjaHood, ItemID.NinjaShirt, ItemID.NinjaPants))
                QuestManager.MarkComplete<GetNinjaSet>();

            return true;
        }
    }
}
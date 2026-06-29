using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class GetBeeSet : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class GetBeeSetCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.BeeGun;

        public override bool? UseItem(Item item, Player player)
        {
            if (player.HasArmorSet(ItemID.BeeHat, ItemID.BeeBreastplate, ItemID.BeeGreaves))
                QuestBooksMod.MarkComplete<GetBeeSet>();

            return true;
        }
    }
}
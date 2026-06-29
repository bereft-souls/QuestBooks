using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class GetPinkPearl : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class GetPinkPearlCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.PinkPearl;

        public override void OnSpawn(Item item, IEntitySource source)
        {
            if (source is not EntitySource_ItemUse usage || usage.Item.type != ItemID.Oyster)
                return;

            QuestBooksMod.MarkComplete<GetPinkPearl>();
        }
    }
}
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class ShimmerTransmute : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class ShimmerTransmuteCheck : GlobalItem
    {
        public override void OnSpawn(Item item, IEntitySource source)
        {
            if (source is not EntitySource_Misc misc || misc.Context != "Shimmer")
                return;

            QuestBooksMod.MarkComplete<ShimmerTransmute>();
        }
    }
}
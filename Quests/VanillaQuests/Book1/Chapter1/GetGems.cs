using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class GetGems : QBQuest
{
    public override QuestType QuestType=> QuestType.Player;

    public override bool CheckCompletion() => false;
    
    // TODO: Possibly make some sort of custom item sets repository where modders can add their own items to existing sets.
    public class GemItemCheck : ModPlayer
    {
        /// <summary>
        ///     Gets a set of items that are considered gems.
        /// </summary>
        public static bool[] Gem { get; private set; }

        public override void SetStaticDefaults()
        {
            Gem = ItemID.Sets.Factory.CreateBoolSet
            (
                ItemID.Diamond,
                ItemID.Amber,
                ItemID.Ruby,
                ItemID.Emerald,
                ItemID.Sapphire,
                ItemID.Topaz,
                ItemID.Amethyst
            );
        }

        public override bool OnPickup(Item item)
        {
            if (Gem[item.type])
            {
                QuestManager.MarkComplete<GetGems>();
            }
            
            return true;
        }
    }
}
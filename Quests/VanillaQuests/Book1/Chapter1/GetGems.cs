using QuestBooks.Sets;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class GetGems : QBQuest
{
    public override bool CheckCompletion() => false;
    
    public class GemItemCheck : ModPlayer
    {
        public override bool OnPickup(Item item)
        {
            if (ItemSetsSystem.Gem[item.type])
                QuestManager.MarkComplete<GetGems>();
            
            return true;
        }
    }
}
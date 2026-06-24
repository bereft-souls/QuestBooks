using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class CraftAnyGear : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftGearCheck() : CraftItemHook<CraftAnyGear>(MatchItem)
    {
        public static bool MatchItem(Item item)
        {
            if (item.pick > 0 || item.hammer > 0 || item.axe > 0)
                return false;

            if (item.defense <= 0 && item.damage <= 0)
                return false;

            return true;
        }
    }
}

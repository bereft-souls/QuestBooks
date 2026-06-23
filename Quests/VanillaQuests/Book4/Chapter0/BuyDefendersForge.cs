using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class BuyDefendersForge : QBQuest
{
    public override bool CheckCompletion() => false;

    public class BuyDefendersForgeCheck() : BuyItemHook<BuyDefendersForge>(ItemID.DefendersForge);
}
using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter2;

public class CraftFrostsparkBoots : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftFrostsparkBootsCheck() : CraftItemHook<CraftFrostsparkBoots>(ItemID.FrostsparkBoots);
}
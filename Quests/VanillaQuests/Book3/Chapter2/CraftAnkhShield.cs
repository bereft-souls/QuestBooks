using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter2;

public class CraftAnkhShield : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftAnkhShieldCheck() : CraftItemHook<CraftAnkhShield>(ItemID.AnkhShield);
}
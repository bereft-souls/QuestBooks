using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftAnvil : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftAnvilCheck() : CraftItemHook<CraftAnvil>(ItemID.IronAnvil);
}
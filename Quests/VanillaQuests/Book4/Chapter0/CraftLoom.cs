using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftLoom : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftLoomCheck() : CraftItemHook<CraftLoom>(ItemID.Loom);
}
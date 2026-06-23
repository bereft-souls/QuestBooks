using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftHardmodeAnvil : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftHardmodeAnvilCheck() : CraftItemHook<CraftHardmodeAnvil>(ItemID.MythrilAnvil, ItemID.OrichalcumAnvil);
}
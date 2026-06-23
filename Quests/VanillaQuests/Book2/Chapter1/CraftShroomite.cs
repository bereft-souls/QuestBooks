using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class CraftShroomite : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftShroomiteCheck() : CraftItemHook<CraftShroomite>(ItemID.ShroomiteBar);
}
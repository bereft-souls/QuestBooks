using QuestBooks.Quests.QuestSystems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class CraftChlorophyteBars : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftChlorophyteBarsCheck() : CraftItemHook<CraftChlorophyteBars>(ItemID.ChlorophyteBar);
}
using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class GetAncientManipulator : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.LunarCraftingStation);
}
using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class GetGoldenFishingRod : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.GoldenFishingRod);
}
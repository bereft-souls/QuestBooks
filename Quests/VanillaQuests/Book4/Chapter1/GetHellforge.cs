using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class GetHellforge : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.Hellforge);
}
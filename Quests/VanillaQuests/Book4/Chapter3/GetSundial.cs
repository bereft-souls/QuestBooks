using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter3;

public class GetSundial : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.Sundial);
}
using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class GetGoldenCarp : QBQuest
{
    public override QuestType QuestType => QuestType.Player;
    
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.GoldenCarp);
}
using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class GetAnglerSet : QBQuest
{
    public override QuestType QuestType => QuestType.Player;
    
    public override bool CheckCompletion() => Main.LocalPlayer.HasAllItems(ItemID.AnglerHat, ItemID.AnglerVest, ItemID.AnglerPants);
}
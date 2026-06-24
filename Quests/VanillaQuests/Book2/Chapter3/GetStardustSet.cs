using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter3;

public class GetStardustSet : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasAllItems(ItemID.StardustHelmet, ItemID.StardustBreastplate, ItemID.StardustLeggings);
}
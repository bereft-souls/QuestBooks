using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter3;

public class GetNebulaSet : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasAllItems(ItemID.NebulaHelmet, ItemID.NebulaBreastplate, ItemID.NebulaLeggings);
}
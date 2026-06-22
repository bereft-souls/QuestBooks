using QuestBooks.Content.Sets;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter2;

public class GetAnyRunningBoots : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasAnyItem(ItemSets.Accessories.Boots);
}
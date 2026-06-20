using QuestBooks.Sets;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter2;

public class GetAnyRunningBoots : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemSetsSystem.Boots.Running);
}
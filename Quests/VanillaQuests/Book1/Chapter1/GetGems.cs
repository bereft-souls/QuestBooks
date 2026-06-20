using QuestBooks.Sets;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class GetGems : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemSetsSystem.Gem);
}
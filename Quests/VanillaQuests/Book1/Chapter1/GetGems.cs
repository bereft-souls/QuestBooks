using QuestBooks.Content.Sets;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class GetGems : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemSetsSystem.Ores.Gem);
}
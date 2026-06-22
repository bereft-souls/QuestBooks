using QuestBooks.Content.Sets;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class GetGems : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasAnyItem(ItemSets.Materials.Gems);
}
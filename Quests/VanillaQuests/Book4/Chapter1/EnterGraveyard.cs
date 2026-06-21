using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class EnterGraveyard : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneGraveyard;
}
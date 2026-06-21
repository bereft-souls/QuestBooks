namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class SubbiomeTemple : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneLihzhardTemple;
}
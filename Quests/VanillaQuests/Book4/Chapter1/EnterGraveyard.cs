namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class EnterGraveyard : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneGraveyard;
}
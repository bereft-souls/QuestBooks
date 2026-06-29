namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class SubbiomeMarble : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneMarble;
}
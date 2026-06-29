namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class SubbiomeGranite : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneGranite;
}
namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class SubbiomeGranite : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneGranite;
}
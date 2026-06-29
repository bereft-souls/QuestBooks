namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class SubbiomeMeteor : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneMeteor;
}
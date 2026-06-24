namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class SubbiomeHive : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneHive;
}
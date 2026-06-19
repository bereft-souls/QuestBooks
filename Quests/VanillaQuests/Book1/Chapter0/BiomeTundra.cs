namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class BiomeTundra : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneSnow;
}
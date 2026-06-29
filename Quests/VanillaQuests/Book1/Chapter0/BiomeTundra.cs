namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class BiomeTundra : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneSnow;
}
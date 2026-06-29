namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class BiomeDesert : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneDesert;
}
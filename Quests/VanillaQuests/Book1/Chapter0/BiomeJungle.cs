namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class BiomeJungle : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneJungle;
}
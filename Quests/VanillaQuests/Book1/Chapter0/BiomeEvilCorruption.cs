namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class BiomeEvilCorruption : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneCorrupt;
}
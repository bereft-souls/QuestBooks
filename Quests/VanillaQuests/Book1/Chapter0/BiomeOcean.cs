namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class BiomeOcean : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneBeach;
}
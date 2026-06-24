namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class BiomeJungle : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneJungle;
}
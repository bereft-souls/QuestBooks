namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class BiomeEvilCrimson : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneCrimson;
}
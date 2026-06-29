namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class BiomeDungeon : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneDungeon;
}
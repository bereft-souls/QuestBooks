namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class BiomeSky : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneSkyHeight;
}
namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class EnterUnderground : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneDirtLayerHeight;
}
namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class EnterUnderground : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneDirtLayerHeight;
}
namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class EnterCaverns : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneRockLayerHeight;
}
namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class EnterCaverns : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneRockLayerHeight;
}
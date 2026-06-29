namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class BiomeHallow : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneHallow;
}
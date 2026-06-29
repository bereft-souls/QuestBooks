namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class GetGoldenFishingRod : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.GoldenFishingRod);
}
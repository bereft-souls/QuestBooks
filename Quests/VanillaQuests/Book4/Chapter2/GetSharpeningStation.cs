namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter2;

public class GetSharpeningStation : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.SharpeningStation);
}
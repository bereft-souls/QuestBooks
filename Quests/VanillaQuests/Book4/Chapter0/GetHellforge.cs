namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class GetHellforge : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.Hellforge);
}
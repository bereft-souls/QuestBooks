namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter2;

public class GetBastStatue : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.CatBast);
}
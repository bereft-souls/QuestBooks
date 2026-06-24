namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter2;

public class GetSundial : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.Sundial);
}
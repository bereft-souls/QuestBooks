namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class GetAlchemyStation : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.AlchemyTable);
}
namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter2;

public class GetBewitchingTable : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.BewitchingTable);
}
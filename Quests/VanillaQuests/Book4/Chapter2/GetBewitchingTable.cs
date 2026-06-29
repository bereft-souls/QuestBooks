namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter2;

public class GetBewitchingTable : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.BewitchingTable);
}
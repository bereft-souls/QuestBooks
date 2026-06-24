namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class GetWarTable : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.WarTable);
}
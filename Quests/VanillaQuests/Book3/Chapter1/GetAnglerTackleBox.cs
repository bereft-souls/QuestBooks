namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class GetAnglerTackleBox : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.AnglerTackleBag);
}
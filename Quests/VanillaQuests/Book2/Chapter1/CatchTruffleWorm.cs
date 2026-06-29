namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class CatchTruffleWorm : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.TruffleWorm);
}
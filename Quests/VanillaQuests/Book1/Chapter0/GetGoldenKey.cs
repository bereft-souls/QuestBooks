namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class GetGoldenKey : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.GoldenKey);
}
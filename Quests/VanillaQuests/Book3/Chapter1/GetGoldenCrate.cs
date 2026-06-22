namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class GetGoldenCrate : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.GoldenCrate);
}
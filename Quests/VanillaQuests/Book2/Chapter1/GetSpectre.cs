namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class GetSpectre : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.SpectreBar);
}
namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class GetTurtleShells : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.TurtleShell);
}
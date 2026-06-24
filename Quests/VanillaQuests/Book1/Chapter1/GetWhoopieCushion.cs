namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class GetWhoopieCushion : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.WhoopieCushion);
}
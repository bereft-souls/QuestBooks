namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class GetWhoopieCushion : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.WhoopieCushion);
}
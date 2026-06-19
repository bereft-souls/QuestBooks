namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class GetAshwood : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    // TODO: Do we want to check if the player has the item or if it actually killed the entity that drops the item?
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.AshWood);
}
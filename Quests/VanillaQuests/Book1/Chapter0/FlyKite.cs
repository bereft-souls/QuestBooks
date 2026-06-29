namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class FlyKite : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => ItemID.Sets.IsAKite[Main.LocalPlayer.HeldItem.type];
}
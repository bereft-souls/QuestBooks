namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class GetEoCSummon : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.SuspiciousLookingEye);
}
namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class UseDemonHeart : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.IsItemSlotUnlockedAndUsable(8);
}
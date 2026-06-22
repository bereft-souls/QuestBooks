namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter3;

public class GetLuminiteBars : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.LunarBar);
}
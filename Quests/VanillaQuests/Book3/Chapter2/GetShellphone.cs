namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter2;

public class GetShellphone : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.Shellphone);
}
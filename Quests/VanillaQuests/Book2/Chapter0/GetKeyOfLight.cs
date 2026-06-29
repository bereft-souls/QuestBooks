namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class GetKeyOfLight : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.LightKey);
}
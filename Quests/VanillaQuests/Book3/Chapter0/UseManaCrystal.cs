namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class UseManaCrystal : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.ConsumedManaCrystals > 0;
}
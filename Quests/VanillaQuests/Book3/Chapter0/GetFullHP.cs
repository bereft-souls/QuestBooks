namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class GetFullHP : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.ConsumedLifeCrystals >= Player.LifeCrystalMax;
}
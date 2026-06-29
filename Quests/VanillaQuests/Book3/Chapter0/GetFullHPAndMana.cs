namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class GetFullHPAndMana : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.ConsumedLifeCrystals >= Player.LifeCrystalMax && Main.LocalPlayer.ConsumedManaCrystals >= Player.ManaCrystalMax;
}
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter2;

public class BuyCrystalBall : QBQuest
{
    public override bool CheckCompletion() => false;

    public class BuyCrystalBallCheck() : BuyItemHook<BuyCrystalBall>(ItemID.CrystalBall);
}
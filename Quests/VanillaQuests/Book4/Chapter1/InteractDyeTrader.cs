using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class InteractDyeTrader : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override void Load() => On_Player.GetDyeTraderReward += Check;

    public override bool CheckCompletion() => false;

    private static void Check(On_Player.orig_GetDyeTraderReward orig, Player self, NPC dyeTrader)
    {
        orig(self, dyeTrader);
        QuestBooksMod.MarkComplete<InteractDyeTrader>();
    }
}
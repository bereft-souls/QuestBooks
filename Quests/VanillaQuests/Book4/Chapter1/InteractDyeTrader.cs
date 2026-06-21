using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class InteractDyeTrader : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class InteractDyeTraderSystem : ModSystem
    {
        public override void Load() => On_Player.GetDyeTraderReward += Check;

        private static void Check(On_Player.orig_GetDyeTraderReward orig, Player self, NPC dyeTrader)
        {
            orig(self, dyeTrader);

            QuestManager.MarkComplete<InteractDyeTrader>();
        }
    }
}
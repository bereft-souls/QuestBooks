using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class KillUndeadMiner : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class KillUndeadMinerCheck() : KillNPCHook<KillUndeadMiner>(NPCID.UndeadMiner);
}
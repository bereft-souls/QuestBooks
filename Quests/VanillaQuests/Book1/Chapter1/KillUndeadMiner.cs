using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class KillUndeadMiner : QBQuest
{
    public override bool CheckCompletion() => false;

    public class KillUndeadMinerCheck() : KillNPCCheck<KillUndeadMiner>(NPCID.UndeadMiner);
}
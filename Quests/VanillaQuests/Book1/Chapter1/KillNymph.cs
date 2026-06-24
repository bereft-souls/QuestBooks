using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class KillNymph : QBQuest
{
    public override bool CheckCompletion() => false;

    public class KillNymphCheck() : KillNPCHook<KillNymph>(NPCID.Nymph);
}
using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class KillNymph : QBQuest
{
    public override bool CheckCompletion() => false;

    public class KillNymphCheck() : KillNPCCheck<KillNymph>(NPCID.Nymph);
}
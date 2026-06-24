using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class KillTim : QBQuest
{
    public override bool CheckCompletion() => false;

    public class KillTimCheck() : KillNPCHook<KillTim>(NPCID.Tim);
}
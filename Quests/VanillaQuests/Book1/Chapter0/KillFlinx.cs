using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class KillFlinx : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class KillFlinxCheck() : KillNPCHook<KillFlinx>(NPCID.SnowFlinx);
}
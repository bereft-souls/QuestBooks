using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class KillFlinx : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class KillFlinxCheck() : KillNPCCheck<KillFlinx>(NPCID.SnowFlinx);
}
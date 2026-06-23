using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatChaosElemental : QBQuest
{
    public override bool CheckCompletion() => false;

    public sealed class KillChaosElemenalCheck() : KillNPCCheck<DefeatChaosElemental>(NPCID.ChaosElemental);
}
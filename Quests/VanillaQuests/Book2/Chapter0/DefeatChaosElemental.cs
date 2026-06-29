using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatChaosElemental : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public sealed class KillChaosElemenalCheck() : KillNPCHook<DefeatChaosElemental>(NPCID.ChaosElemental);
}
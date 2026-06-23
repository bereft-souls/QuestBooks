using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatGoblinSummoner : QBQuest
{
    public override bool CheckCompletion() => false;

    public sealed class KillGoblinSummonerCheck() : KillNPCCheck<DefeatGoblinSummoner>(NPCID.GoblinSummoner);
}
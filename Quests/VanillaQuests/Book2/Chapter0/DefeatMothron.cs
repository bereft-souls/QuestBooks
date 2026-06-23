using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatMothron : QBQuest
{
    public override bool CheckCompletion() => false;

    public sealed class KillMothronCheck() : KillNPCHook<DefeatMothron>(NPCID.Mothron);
}
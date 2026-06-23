using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatWyvern : QBQuest
{
    public override bool CheckCompletion() => false;

    public sealed class KillWyvernCheck() : KillNPCCheck<DefeatWyvern>(NPCID.WyvernHead);
}
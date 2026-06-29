using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatWyvern : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public sealed class KillWyvernCheck() : KillNPCHook<DefeatWyvern>(NPCID.WyvernHead);
}
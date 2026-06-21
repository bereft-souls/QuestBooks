using QuestBooks.Common.NPCs;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatChaosElemental : QBQuest
{
    public override bool CheckCompletion() => NPCDownedFlagsSystem.Downed(NPCID.ChaosElemental);
}
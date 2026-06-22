using QuestBooks.Systems.Common.NPCs;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class DefeatBetsy : QBQuest
{
    public override bool CheckCompletion() => NPCDownedFlagsSystem.Downed(NPCID.DD2Betsy);
}
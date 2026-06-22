using QuestBooks.Common.NPCs;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatGoblinSummoner : QBQuest
{
    public override bool CheckCompletion() => NPCDownedFlagsSystem.Downed(NPCID.GoblinSummoner);
}
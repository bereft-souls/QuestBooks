using QuestBooks.Common.NPCs;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class KillMarriage : QBQuest
{
    public override bool CheckCompletion() => NPCDownedFlagsSystem.DownedAll(NPCID.TheGroom, NPCID.TheBride);
}
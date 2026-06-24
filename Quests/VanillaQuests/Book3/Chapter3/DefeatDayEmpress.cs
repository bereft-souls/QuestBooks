using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class DefeatDayEmpress : QBQuest
{
    public override bool CheckCompletion() => false;

    public class DayEmpressOfLightCheck() : KillNPCHook(npc => Match(npc, NPCID.HallowBoss), CheckDaytimeEOL)
    {
        private static void CheckDaytimeEOL(NPC npc)
        {
            if (npc.type != NPCID.HallowBoss || !npc.AI_120_HallowBoss_IsGenuinelyEnraged())
                return;

            QuestManager.MarkComplete<DefeatDayEmpress>();
        }
    }
}
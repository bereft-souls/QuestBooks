using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class DefeatDayEmpress : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class DayEmpressOfLightCheck() : KillNPCHook(npc => Match(npc, NPCID.HallowBoss), CheckDaytimeEOL)
    {
        private static void CheckDaytimeEOL(NPC npc)
        {
            if (!npc.AI_120_HallowBoss_IsGenuinelyEnraged())
                return;

            QuestBooksMod.CompleteQuest<DefeatDayEmpress>();
        }
    }
}
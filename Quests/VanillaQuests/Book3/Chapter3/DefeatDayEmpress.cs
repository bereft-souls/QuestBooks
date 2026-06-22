using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class DefeatDayEmpress : QBQuest
{
    public override bool CheckCompletion() => false;

    public class DayEmpressOfLightCheck : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.type != NPCID.HallowBoss)
                return;

            if (!npc.AI_120_HallowBoss_IsGenuinelyEnraged())
                return;

            QuestManager.CompleteQuest<DefeatDayEmpress>();
        }
    }
}
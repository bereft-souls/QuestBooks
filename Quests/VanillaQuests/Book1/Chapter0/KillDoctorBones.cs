using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class KillDoctorBones : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class KillDoctorBonesCheck : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.DoctorBones;

        public override void OnKill(NPC npc) => QuestManager.MarkComplete<KillDoctorBones>();
    }
}
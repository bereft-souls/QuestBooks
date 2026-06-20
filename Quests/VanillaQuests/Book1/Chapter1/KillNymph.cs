using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class KillNymph : QBQuest
{
    public override bool CheckCompletion() => false;

    public class KillNymphCheck : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Nymph;

        public override void OnKill(NPC npc) => QuestManager.MarkComplete<KillNymph>();
    }
}
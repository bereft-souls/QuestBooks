using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class KillUndeadMiner : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class UndeadMinerNPCCheck : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.UndeadMiner;

        public override void OnKill(NPC npc) => QuestManager.MarkComplete<KillUndeadMiner>();
    }
}
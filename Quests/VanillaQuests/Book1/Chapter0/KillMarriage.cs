namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class KillMarriage : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class KillMarriageCheck : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.TheGroom || entity.type == NPCID.TheBride;

        // TODO: Implementation.
        public override void OnKill(NPC npc) { }
    }
}
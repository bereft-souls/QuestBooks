using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class PlanteraDefeated : Quest
    {
        public override bool CheckCompletion() => NPC.downedPlantBoss;
    }
}

using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class EmpressOfLightDefeated : Quest
    {
        public override bool CheckCompletion() => NPC.downedEmpressOfLight;
    }
}

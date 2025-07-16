using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class LunaticCultistDefeated : Quest
    {
        public override bool CheckCompletion() => NPC.downedAncientCultist;
    }
}

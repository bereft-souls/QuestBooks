using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class KingSlimeDefeated : Quest
    {
        public override bool CheckCompletion() => NPC.downedSlimeKing;
    }
}

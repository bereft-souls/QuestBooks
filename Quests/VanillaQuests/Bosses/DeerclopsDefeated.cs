using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class DeerclopsDefeated : Quest
    {
        public override bool CheckCompletion() => NPC.downedDeerclops;
    }
}

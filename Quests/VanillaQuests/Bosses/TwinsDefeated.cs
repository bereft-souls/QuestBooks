using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class TwinsDefeated : Quest
    {
        public override bool CheckCompletion() => NPC.downedMechBoss2;
    }
}

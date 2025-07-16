using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class DukeFishronDefeated : Quest
    {
        public override bool CheckCompletion() => NPC.downedFishron;
    }
}

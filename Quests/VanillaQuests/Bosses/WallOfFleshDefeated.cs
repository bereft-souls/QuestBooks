using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Bosses
{
    public class WallOfFleshDefeated : Quest
    {
        public override bool CheckCompletion() => Main.hardMode;
    }
}

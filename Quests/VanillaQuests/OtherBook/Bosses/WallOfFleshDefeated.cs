using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class WallOfFleshDefeated : QBQuest
    {
        public override bool CheckCompletion() => Main.hardMode;
    }
}

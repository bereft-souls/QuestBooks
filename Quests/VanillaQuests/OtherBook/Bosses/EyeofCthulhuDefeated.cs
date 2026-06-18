using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class EyeofCthulhuDefeated : QBQuest
    {
        public override bool CheckCompletion() => NPC.downedBoss1;
    }
}

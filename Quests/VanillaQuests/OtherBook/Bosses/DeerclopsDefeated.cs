using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class DeerclopsDefeated : QBQuest
    {
        public override bool CheckCompletion() => NPC.downedDeerclops;
    }
}

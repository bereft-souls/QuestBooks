using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class EyeofCthulhu : QBQuest
    {
        public override bool CheckCompletion() => NPC.downedBoss1;
    }
}

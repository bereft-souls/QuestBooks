using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class Skeletron : QBQuest
    {
        public override bool CheckCompletion() => NPC.downedBoss3;
    }
}

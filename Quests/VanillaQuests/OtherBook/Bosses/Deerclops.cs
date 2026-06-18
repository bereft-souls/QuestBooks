using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class Deerclops : QBQuest
    {
        public override bool CheckCompletion() => NPC.downedDeerclops;
    }
}

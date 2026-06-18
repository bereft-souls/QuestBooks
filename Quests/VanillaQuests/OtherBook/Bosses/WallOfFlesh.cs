using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses
{
    public class WallofFlesh : QBQuest
    {
        public override bool CheckCompletion() => Main.hardMode;
    }
}

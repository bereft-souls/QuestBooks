using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class KingSlime : QBQuest
{
    public override bool CheckCompletion() => NPC.downedSlimeKing;
}

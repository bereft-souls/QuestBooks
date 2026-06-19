using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class KingSlimeDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedSlimeKing;
}

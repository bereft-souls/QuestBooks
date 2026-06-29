namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class KingSlimeDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedSlimeKing;
}

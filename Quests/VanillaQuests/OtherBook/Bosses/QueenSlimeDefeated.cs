namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class QueenSlimeDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedQueenSlime;
}

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class QueenSlimeDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedQueenSlime;
}

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class PiratesDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedPirates;
}

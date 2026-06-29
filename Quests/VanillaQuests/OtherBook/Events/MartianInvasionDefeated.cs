namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class MartianInvasionDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedMartians;
}

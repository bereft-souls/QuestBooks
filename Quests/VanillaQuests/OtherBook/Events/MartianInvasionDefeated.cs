namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class MartianInvasionDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedMartians;
}

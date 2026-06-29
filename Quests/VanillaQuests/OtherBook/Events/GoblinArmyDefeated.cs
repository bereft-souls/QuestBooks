namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class GoblinArmyDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedGoblins;
}

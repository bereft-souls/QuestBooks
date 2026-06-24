namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class DukeFishronDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedFishron;
}

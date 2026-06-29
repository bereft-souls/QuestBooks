namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class DukeFishronDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedFishron;
}

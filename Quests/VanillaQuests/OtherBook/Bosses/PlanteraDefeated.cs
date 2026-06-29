namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class PlanteraDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedPlantBoss;
}

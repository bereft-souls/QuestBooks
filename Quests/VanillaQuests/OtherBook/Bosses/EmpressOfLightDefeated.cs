namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class EmpressOfLightDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedEmpressOfLight;
}

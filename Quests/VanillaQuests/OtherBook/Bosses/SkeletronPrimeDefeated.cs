namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class SkeletronPrimeDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedMechBoss3;
}

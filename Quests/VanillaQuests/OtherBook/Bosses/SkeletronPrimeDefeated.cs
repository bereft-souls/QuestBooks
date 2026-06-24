namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class SkeletronPrimeDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedMechBoss3;
}

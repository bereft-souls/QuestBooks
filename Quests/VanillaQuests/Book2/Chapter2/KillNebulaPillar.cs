namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter2;

public class KillNebulaPillar : QBQuest
{
    public override bool CheckCompletion() => NPC.downedTowerNebula;
}
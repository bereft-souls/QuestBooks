namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class GardenGnome : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;
}
namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class PurifyWorld : QBQuest
{
    public override bool CheckCompletion() => WorldGen.totalEvil == 0;
}
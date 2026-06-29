namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class UsePeddlersSatchel : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => NPC.peddlersSatchelWasUsed;
}
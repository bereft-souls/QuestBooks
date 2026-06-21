namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class UseAegisFruit : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.usedAegisFruit;
}
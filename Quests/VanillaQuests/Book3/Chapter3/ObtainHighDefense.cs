namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class ObtainHighDefense : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.statDefense >= 100;
}
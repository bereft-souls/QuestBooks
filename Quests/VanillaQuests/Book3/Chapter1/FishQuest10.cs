namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class FishQuest10 : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.anglerQuestsFinished >= 10;
}
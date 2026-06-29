namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class FishQuest25 : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.anglerQuestsFinished >= 25;
}
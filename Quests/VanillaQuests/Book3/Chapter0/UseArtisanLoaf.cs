namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class UseArtisanLoaf : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.ateArtisanBread;
}
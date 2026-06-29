namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class Bestiary50 : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.GetBestiaryProgressReport().CompletionPercent >= 0.5f;
}
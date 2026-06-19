namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class ReachRockBottom : QBQuest
{
    // Hardcoded threshold adapted directly from Player::BordersMovement().
    public override bool CheckCompletion() => Main.LocalPlayer.position.Y >= Main.bottomWorld - 640f - 32f - Main.LocalPlayer.height;
}
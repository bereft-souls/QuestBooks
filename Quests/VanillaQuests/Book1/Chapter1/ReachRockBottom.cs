namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class ReachRockBottom : VanillaQuest
{
    // Hardcoded threshold adapted directly from Player::BordersMovement().
    public override bool CheckCompletion() => Main.LocalPlayer.position.Y >= Main.bottomWorld - 672f - Main.LocalPlayer.height;
}
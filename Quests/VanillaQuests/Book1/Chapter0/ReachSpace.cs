namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class ReachSpace : VanillaQuest
{
    // Hardcoded threshold adapted directly from Player::BordersMovement().
    public override bool CheckCompletion() => Main.LocalPlayer.position.Y <= Main.topWorld + 656f;
}
using Terraria.GameContent;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class PlaceTundraPylon : QBQuest
{
    public override bool CheckCompletion() => Main.PylonSystem.HasPylonOfType(TeleportPylonType.Snow);
}
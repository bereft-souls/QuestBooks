using Terraria.GameContent;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class PlaceOceanPylon : QBQuest
{
    public override bool CheckCompletion() => Main.PylonSystem.HasPylonOfType(TeleportPylonType.Beach);
}
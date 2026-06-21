using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria.GameContent;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class PlaceDesertPylon : QBQuest
{
    public override bool CheckCompletion() => Main.PylonSystem.HasPylonOfType(TeleportPylonType.Desert);
}
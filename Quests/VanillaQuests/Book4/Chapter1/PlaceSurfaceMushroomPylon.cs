using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria.GameContent;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class PlaceSurfaceMushroomPylon : QBQuest
{
    public override bool CheckCompletion() => Main.PylonSystem.HasPylonOfType(TeleportPylonType.GlowingMushroom);
}
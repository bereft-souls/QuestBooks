using System.Linq;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class EstablishHouse : QBQuest
{
    public override bool CheckCompletion() => Main.npc.Any(n => n.active && n.townNPC && WorldGen.TownManager.HasRoomQuick(n.type));
}

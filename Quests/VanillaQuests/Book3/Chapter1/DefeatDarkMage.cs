using QuestBooks.Quests.QuestSystems;
using System.Linq;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class DefeatDarkMage : QBQuest
{
    // Not affected by other mods, does not need to be a set
    public static readonly int[] DarkMageTypes = [
        NPCID.DD2DarkMageT1,
        NPCID.DD2DarkMageT3
    ];

    public override bool CheckCompletion() => false;

    public sealed class KillDarkMageCheck() : KillNPCHook<DefeatDarkMage>(npc => DarkMageTypes.Contains(npc.type));
}
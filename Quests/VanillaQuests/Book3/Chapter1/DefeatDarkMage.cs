using QuestBooks.Quests.QuestSystems;
using System.Linq;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class DefeatDarkMage : QBQuest
{
    public static readonly int[] DarkMageTypes = [
        NPCID.DD2DarkMageT1,
        NPCID.DD2DarkMageT3
    ];

    public override bool CheckCompletion() => false;

    public sealed class KillDarkMageCheck() : KillNPCCheck<DefeatDarkMage>(npc => DarkMageTypes.Contains(npc.type));
}
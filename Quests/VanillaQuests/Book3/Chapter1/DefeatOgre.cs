using QuestBooks.Quests.QuestSystems;
using System.Linq;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class DefeatOgre : QBQuest
{
    public static readonly int[] OgreTypes = [
        NPCID.DD2OgreT2,
        NPCID.DD2OgreT2
    ];

    public override bool CheckCompletion() => false;

    public sealed class KillOgreCheck() : KillNPCCheck<DefeatOgre>(npc => OgreTypes.Contains(npc.type));
}
using QuestBooks.Quests.QuestSystems;
using System.Linq;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatBiomeMimic : QBQuest
{
    public static readonly int[] BiomeMimicTypes = [
        NPCID.BigMimicCorruption,
        NPCID.BigMimicCrimson,
        NPCID.BigMimicHallow,
        NPCID.BigMimicJungle
    ];

    public override bool CheckCompletion() => false;

    public sealed class KillBigMimicCheck() : KillNPCCheck<DefeatBiomeMimic>(npc => BiomeMimicTypes.Contains(npc.type));
}
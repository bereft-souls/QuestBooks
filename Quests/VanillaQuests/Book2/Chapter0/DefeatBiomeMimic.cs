using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

[ReinitializeDuringResizeArrays]
public class DefeatBiomeMimic : QBQuest
{
    static DefeatBiomeMimic()
    {
        BiomeMimics = NPCID.Sets.Factory.CreateNamedSet("BiomeMimics")
            .Description("All biome mimics")
            .RegisterBoolSet(
                NPCID.BigMimicCorruption,
                NPCID.BigMimicCrimson,
                NPCID.BigMimicHallow,
                NPCID.BigMimicJungle
            );
    }

    public static readonly bool[] BiomeMimics;

    public override bool CheckCompletion() => false;

    public sealed class KillBigMimicCheck() : KillNPCHook<DefeatBiomeMimic>(npc => BiomeMimics[npc.type]);
}
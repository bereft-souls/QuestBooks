using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatBiomeMimic : QBQuest
{
    public static readonly bool[] BiomeMimics = NPCID.Sets.Factory.CreateNamedSet("BiomeMimics")
        .Description("All biome mimics")
        .RegisterBoolSet(
            NPCID.BigMimicCorruption,
            NPCID.BigMimicCrimson,
            NPCID.BigMimicHallow,
            NPCID.BigMimicJungle
        );

    public override bool CheckCompletion() => false;

    public sealed class KillBigMimicCheck() : KillNPCCheck<DefeatBiomeMimic>(npc => BiomeMimics[npc.type]);
}
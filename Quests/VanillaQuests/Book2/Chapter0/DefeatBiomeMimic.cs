using QuestBooks.Common.NPCs;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatBiomeMimic : QBQuest
{
    public override bool CheckCompletion() => NPCDownedFlagsSystem.DownedAny(NPCID.BigMimicCorruption, NPCID.BigMimicCrimson, NPCID.BigMimicHallow, NPCID.BigMimicJungle);
}
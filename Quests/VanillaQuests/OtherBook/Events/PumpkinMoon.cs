using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class PumpkinMoonDefeated : VanillaQuest
{
    public override bool CheckCompletion() => Main.pumpkinMoon && NPC.waveNumber >= 15;
}

public class PumpkingDefeated : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class PumpkingCheck() : KillNPCHook<PumpkingDefeated>(NPCID.Pumpking);
}

public class MourningWoodDefeated : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class MourningWoodCheck() : KillNPCHook<MourningWoodDefeated>(NPCID.MourningWood);
}
using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class AllPillarsDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedTowers;
}

public class SolarPillarDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedTowerSolar;
}

public class VortexPillarDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedTowerVortex;
}

public class NebulaPillarDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedTowerNebula;
}

public class StardustPillarDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedTowerStardust;
}
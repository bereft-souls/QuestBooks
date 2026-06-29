namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class AllPillarsDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedTowers;
}

public class SolarPillarDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedTowerSolar;
}

public class VortexPillarDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedTowerVortex;
}

public class NebulaPillarDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedTowerNebula;
}

public class StardustPillarDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedTowerStardust;
}
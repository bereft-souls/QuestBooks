using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class FrostMoonDefeated : VanillaQuest
{
    public override bool CheckCompletion() => Main.snowMoon && NPC.waveNumber >= 15;
}

public class IceQueenDefeated : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class IceQueenCheck() : KillNPCHook<IceQueenDefeated>(NPCID.IceQueen);
}

public class SantaNK1Defeated : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class SantaNK1Check() : KillNPCHook<SantaNK1Defeated>(NPCID.SantaNK1);
}

public class EverscreamDefeated : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class EverscreamCheck() : KillNPCHook<EverscreamDefeated>(NPCID.Everscream);
}
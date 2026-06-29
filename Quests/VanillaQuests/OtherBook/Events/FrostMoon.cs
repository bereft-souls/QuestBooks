using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class FrostMoonDefeated : VanillaQuest
{
    public override bool CheckCompletion() => Main.snowMoon && NPC.waveNumber >= 15;
}

public class IceQueenDefeated : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class IceQueenCheck : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.type != NPCID.IceQueen)
                return;

            QuestBooksMod.CompleteQuest<IceQueenDefeated>();
        }
    }
}

public class SantaNK1Defeated : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class SantaNK1Check : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.type != NPCID.SantaNK1)
                return;

            QuestBooksMod.CompleteQuest<SantaNK1Defeated>();
        }
    }
}

public class EverscreamDefeated : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class EverscreamCheck() : KillNPCHook<EverscreamDefeated>(NPCID.Everscream);
}
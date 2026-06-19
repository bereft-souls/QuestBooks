using QuestBooks.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class PumpkinMoonDefeated : QBQuest
{
    public override bool CheckCompletion() => Main.pumpkinMoon && NPC.waveNumber >= 15;
}

public class PumpkingDefeated : QBQuest
{
    public override bool CheckCompletion() => false;

    public class PumpkingCheck : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.type != NPCID.Pumpking)
                return;

            QuestManager.CompleteQuest<PumpkingDefeated>();
        }
    }
}

public class MourningWoodDefeated : QBQuest
{
    public override bool CheckCompletion() => false;

    public class MourningWoodCheck : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.type != NPCID.MourningWood)
                return;

            QuestManager.CompleteQuest<MourningWoodDefeated>();
        }
    }
}
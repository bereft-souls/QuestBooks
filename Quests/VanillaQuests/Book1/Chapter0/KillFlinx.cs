using QuestBooks.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class KillFlinx : QBQuest
{
    public override bool CheckCompletion() => false;

    public class FlinxNPCCheck : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.SnowFlinx;

        public override void OnKill(NPC npc) => QuestManager.MarkComplete<KillFlinx>();
    }
}
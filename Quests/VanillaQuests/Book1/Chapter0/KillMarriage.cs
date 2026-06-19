using QuestBooks.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class KillMarriage : QBQuest
{
    public override bool CheckCompletion() => false;
    
    public class MarriageNPCCheck : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.TheGroom || entity.type == NPCID.TheBride;
        }

        public override void OnKill(NPC npc)
        {
            
        }
    }
}
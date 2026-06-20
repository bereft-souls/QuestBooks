using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class KillHardmodeBloodMoonReel : QBQuest
{
    public override bool CheckCompletion() => false;
    
    public class KillHardmodeBloodMoonReelCheck : GlobalItem
    {
        // TODO: Should validation occur when you acquire all item types or just one of them?
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.DripplerFlail || entity.type == ItemID.SharpTears || entity.type == ItemID.BloodHamaxe;

        public override void OnSpawn(Item item, IEntitySource source)
        {
            if (source is not EntitySource_Loot loot || loot.Entity is not NPC npc || npc.type != NPCID.BloodEelHead || npc.type != NPCID.GoblinShark)
                return;

            QuestManager.MarkComplete<KillHardmodeBloodMoonReel>();
        }
    }
}
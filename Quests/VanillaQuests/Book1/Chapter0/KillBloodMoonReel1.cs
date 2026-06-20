using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class KillBloodMoonReel1 : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class KillBloodMoonReel1Check : GlobalItem
    {
        // TODO: Should validation occur when you acquire all item types or just one of them?
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.VampireFrogStaff || entity.type == ItemID.BloodRainBow || entity.type == ItemID.BloodFishingRod;

        public override void OnSpawn(Item item, IEntitySource source)
        {
            if (source is not EntitySource_Loot loot || loot.Entity is not NPC npc || npc.type != NPCID.EyeballFlyingFish || npc.type != NPCID.ZombieMerman)
                return;

            QuestManager.MarkComplete<KillBloodMoonReel1>();
        }
    }
}
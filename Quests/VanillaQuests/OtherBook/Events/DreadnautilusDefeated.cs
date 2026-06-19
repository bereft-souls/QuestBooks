using QuestBooks.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class DreadnautilusDefeated : QBQuest
{
    public override bool CheckCompletion() => false;

    public class DreadnautilusCheck : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.type != NPCID.BloodNautilus)
                return;

            QuestManager.CompleteQuest<DreadnautilusDefeated>();
        }
    }
}

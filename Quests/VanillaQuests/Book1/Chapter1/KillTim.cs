using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class KillTim : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class TimNPCCheck : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Tim;

        public override void OnKill(NPC npc) => QuestManager.MarkComplete<KillTim>();
    }
}
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class DreadnautilusDefeated : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class DreadnautilusCheck : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.type != NPCID.BloodNautilus)
                return;

            QuestBooksMod.CompleteQuest<DreadnautilusDefeated>();
        }
    }
}

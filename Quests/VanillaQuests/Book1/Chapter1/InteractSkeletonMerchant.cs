using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class InteractSkeletonMerchant : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class InteractSkeletonMerchantCheck : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.SkeletonMerchant;

        public override void OnChatButtonClicked(NPC npc, bool firstButton) => QuestManager.MarkComplete<InteractSkeletonMerchant>();
    }
}
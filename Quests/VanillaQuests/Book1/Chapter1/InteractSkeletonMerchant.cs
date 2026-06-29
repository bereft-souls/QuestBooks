using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class InteractSkeletonMerchant : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class InteractSkeletonMerchantCheck() : ChatNPCHook<InteractSkeletonMerchant>(NPCID.SkeletonMerchant);
}
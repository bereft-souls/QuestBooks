using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class InteractSkeletonMerchant : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class InteractSkeletonMerchantCheck() : ChatNPCHook<InteractSkeletonMerchant>(NPCID.SkeletonMerchant);
}

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class ShimmerNPCAlt : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => AnyNPCs(static npc => npc.IsShimmerVariant);
}
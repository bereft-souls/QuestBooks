using QuestBooks.Common.NPCs;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class ShimmerNPCAlt : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => NPCUtilities.AnyNPCs(static npc => npc.IsShimmerVariant);
}
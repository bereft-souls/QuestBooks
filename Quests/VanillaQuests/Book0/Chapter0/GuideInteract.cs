using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class GuideInteract : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class GuideInteractCheck() : ChatNPCHook<GuideInteract>(NPCID.Guide);
}

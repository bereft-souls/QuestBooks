using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class GuideInteract : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class GuideInteractCheck() : ChatNPCHook<GuideInteract>(NPCID.Guide);
}

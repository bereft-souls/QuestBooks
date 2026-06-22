using QuestBooks.Content.Sets;
using QuestBooks.Systems.Common.NPCs;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class DefeatEverySlime : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => NPCDownedFlagsSystem.DownedAll(NPCSets.Enemies.Slimes);
}
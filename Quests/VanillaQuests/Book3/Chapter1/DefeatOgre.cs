using QuestBooks.Common.NPCs;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class DefeatOgre : QBQuest
{
    public override bool CheckCompletion() => NPCDownedFlagsSystem.DownedAny(NPCID.DD2OgreT2, NPCID.DD2OgreT3);
}
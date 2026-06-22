using QuestBooks.Systems.Common.NPCs;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class DefeatDarkMage : QBQuest
{
    public override bool CheckCompletion() => NPCDownedFlagsSystem.DownedAny(NPCID.DD2DarkMageT1, NPCID.DD2DarkMageT3);
}
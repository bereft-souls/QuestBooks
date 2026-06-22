using QuestBooks.Systems.Common.NPCs;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatWeatherMiniboss : QBQuest
{
    public override bool CheckCompletion() => NPCDownedFlagsSystem.DownedAny(NPCID.SandElemental, NPCID.IceGolem);
}
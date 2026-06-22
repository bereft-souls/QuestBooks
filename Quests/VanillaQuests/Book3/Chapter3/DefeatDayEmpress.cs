using QuestBooks.Common.NPCs;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class DefeatDayEmpress : QBQuest
{
    public override bool CheckCompletion() => DayEmpressOfLightSystem.Downed;
}
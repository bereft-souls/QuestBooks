using QuestBooks.Quests.QuestSystems;
using System.Linq;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatWeatherMiniboss : QBQuest
{
    public static readonly int[] WeatherMinibossTypes = [
        NPCID.SandElemental,
        NPCID.IceGolem
    ];

    public override bool CheckCompletion() => false;

    public sealed class KillWeatherMinibossCheck() : KillNPCCheck<DefeatWeatherMiniboss>(npc => WeatherMinibossTypes.Contains(npc.type));
}
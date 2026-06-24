using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatWeatherMiniboss : QBQuest
{
    public static readonly bool[] WeatherMinibossTypes = NPCID.Sets.Factory.CreateNamedSet("WeatherMinibosses")
        .Description("Minibosses that only spawn during weather events")
        .RegisterBoolSet(
            NPCID.SandElemental,
            NPCID.IceGolem
        );

    public override bool CheckCompletion() => false;

    public sealed class KillWeatherMinibossCheck() : KillNPCHook<DefeatWeatherMiniboss>(npc => WeatherMinibossTypes[npc.type]);
}
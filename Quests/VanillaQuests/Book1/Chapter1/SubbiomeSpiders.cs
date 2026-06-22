using QuestBooks.Content.Biomes;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class SubbiomeSpiders : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.InModBiome<SpiderNestBiome>();
}
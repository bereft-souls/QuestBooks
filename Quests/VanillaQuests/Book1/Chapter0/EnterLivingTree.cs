using QuestBooks.Content.Biomes;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class EnterLivingTree : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.InModBiome<LivingTreeBiome>();
}
using QuestBooks.Systems;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

// Used only in the "hidden quest" element
public class TrunkCompleteCheck : Quest
{
    public override bool CheckCompletion() =>
        QuestManager.GetQuest<EstablishHouse>().Completed &&
        QuestManager.GetQuest<CraftHammer>().Completed &&
        QuestManager.GetQuest<CraftWorkbench>().Completed &&
        QuestManager.GetQuest<ChopTree>().Completed &&
        QuestManager.GetQuest<GuideInteract>().Completed &&
        QuestManager.GetQuest<ForestBiome>().Completed &&
        QuestManager.GetQuest<StartQBInfo>().Completed &&
        QuestManager.GetQuest<StartFirstSteps>().Completed;
}

public class BasicsCompleteInfo : InfoQuest;
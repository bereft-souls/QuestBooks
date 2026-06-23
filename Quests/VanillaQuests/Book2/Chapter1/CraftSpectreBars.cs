using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter1;

public class CraftSpectreBars : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftSpectreBarsCheck() : CraftItemHook<CraftSpectreBars>(ItemID.SpectreBar);
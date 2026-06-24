using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftHardmodeFurnace : QBQuest
{
    public override bool CheckCompletion() => false;

    public class CraftHardmodeFurnaceCheck() : CraftItemHook<CraftHardmodeFurnace>(ItemID.AdamantiteForge, ItemID.TitaniumForge);
}
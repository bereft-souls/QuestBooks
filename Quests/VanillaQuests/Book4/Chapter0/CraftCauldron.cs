using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter0;

public class CraftCauldron : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class CraftCauldronCheck() : CraftItemHook<CraftCauldron>(ItemID.Cauldron);
}
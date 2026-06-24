using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class CraftMoltenPickaxe : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftMoltenPickaxeCheck() : CraftItemHook<CraftMoltenPickaxe>(ItemID.MoltenPickaxe);
}
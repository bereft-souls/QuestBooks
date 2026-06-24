using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

[ReinitializeDuringResizeArrays]
public class CraftEvilPickaxe : QBQuest
{
    static CraftEvilPickaxe()
    {
        EvilPickaxes = ItemID.Sets.Factory.CreateNamedSet("EvilPickaxes")
            .Description("Pickaxes that are crafted from world evil materials")
            .RegisterBoolSet(ItemID.DeathbringerPickaxe, ItemID.NightmarePickaxe);
    }

    public static readonly bool[] EvilPickaxes;

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftEvilPickaxeCheck() : CraftItemHook<CraftEvilPickaxe>(EvilPickaxes);
}
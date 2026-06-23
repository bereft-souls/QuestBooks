using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class CraftEvilPickaxe : QBQuest
{
    public static readonly bool[] EvilPickaxes = ItemID.Sets.Factory.CreateNamedSet("EvilPickaxes")
        .Description("Pickaxes that are crafted from world evil materials")
        .RegisterBoolSet(
            ItemID.DeathbringerPickaxe,
            ItemID.NightmarePickaxe
        );
    
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftEvilPickaxeCheck() : CraftItemHook<CraftEvilPickaxe>(EvilPickaxes);
}
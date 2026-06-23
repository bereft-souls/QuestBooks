using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class CraftHallowedPickaxe : QBQuest
{
    public static readonly bool[] HallowedPickaxes = ItemID.Sets.Factory.CreateNamedSet("HallowedPickaxes")
        .Description("Pickaxes that are (intended to be) hallowed exclusive")
        .RegisterBoolSet(
            ItemID.Drax,
            ItemID.PickaxeAxe
        );
    
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftHallowedPickaxeCheck() : CraftItemHook<CraftHallowedPickaxe>(HallowedPickaxes);
}
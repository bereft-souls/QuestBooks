
using QuestBooks.Utilities;
using System.Linq;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter2;

public class GetAnyRunningBoots : QBQuest
{
    public static readonly bool[] Boots = ItemID.Sets.Factory.CreateNamedSet("Boots")
        .Description("Boots-type accessories")
        .RegisterBoolSet(
            ItemID.SpectreBoots,
            ItemID.FrostsparkBoots,
            ItemID.SailfishBoots,
            ItemID.LavaWaders,
            ItemID.HermesBoots,
            ItemID.FlurryBoots,
            ItemID.LightningBoots,
            ItemID.TerrasparkBoots
        );

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.EnumerateInventory().Any(slot => Boots[slot.Item.type]);
}
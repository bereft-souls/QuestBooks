using QuestBooks.Utilities;
using System.Linq;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class GetGems : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public static readonly bool[] Gems = ItemID.Sets.Factory.CreateNamedSet("Gems")
        .Description("All types of gems")
        .RegisterBoolSet(
            ItemID.Diamond,
            ItemID.Amber,
            ItemID.Ruby,
            ItemID.Emerald,
            ItemID.Sapphire,
            ItemID.Topaz,
            ItemID.Amethyst
        );

    public override bool CheckCompletion() => Main.LocalPlayer.HasAnyItem(Gems);
}
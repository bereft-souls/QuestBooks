using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class CraftEvilBars : QBQuest
{
    public static readonly bool[] EvilBars = ItemID.Sets.Factory.CreateNamedSet("EvilBars")
        .Description("Any world evil bar")
        .RegisterBoolSet(ItemID.CrimtaneBar, ItemID.DemoniteBar);

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftEvilBarsCheck() : CraftItemHook<CraftEvilBars>(EvilBars);
}
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class GetAnyOldOnesArmySet : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasAllItems(ItemID.ApprenticeHat, ItemID.ApprenticeRobe, ItemID.ApprenticeTrousers)
        || Main.LocalPlayer.HasAllItems(ItemID.HuntressWig, ItemID.HuntressJerkin, ItemID.HuntressPants)
        || Main.LocalPlayer.HasAllItems(ItemID.MonkBrows, ItemID.MonkShirt, ItemID.MonkPants)
        || Main.LocalPlayer.HasAllItems(ItemID.SquireGreatHelm, ItemID.SquirePlating, ItemID.SquireGreaves)
        || Main.LocalPlayer.HasAllItems(ItemID.ApprenticeAltHead, ItemID.ApprenticeAltShirt, ItemID.ApprenticeAltPants)
        || Main.LocalPlayer.HasAllItems(ItemID.HuntressAltHead, ItemID.HuntressAltShirt, ItemID.HuntressAltPants)
        || Main.LocalPlayer.HasAllItems(ItemID.MonkAltHead, ItemID.MonkAltShirt, ItemID.MonkAltPants)
        || Main.LocalPlayer.HasAllItems(ItemID.SquireAltHead, ItemID.SquireAltShirt, ItemID.SquireAltPants);
}
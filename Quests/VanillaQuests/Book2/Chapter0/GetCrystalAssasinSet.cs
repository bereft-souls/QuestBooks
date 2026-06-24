using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class GetCrystalAssasinSet : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasAllItems(ItemID.CrystalNinjaHelmet, ItemID.CrystalNinjaChestplate, ItemID.CrystalNinjaLeggings);
}
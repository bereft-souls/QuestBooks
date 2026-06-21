using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class GetSnowArmor : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasAllItems(ItemID.EskimoHood, ItemID.EskimoCoat, ItemID.EskimoPants);
}
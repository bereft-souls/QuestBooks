using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class GetOrePickaxe : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasAnyItem
    (
        ItemID.IronPickaxe,
        ItemID.LeadPickaxe,
        ItemID.SilverPickaxe,
        ItemID.TungstenPickaxe,
        ItemID.GoldPickaxe,
        ItemID.PlatinumPickaxe
    );
}
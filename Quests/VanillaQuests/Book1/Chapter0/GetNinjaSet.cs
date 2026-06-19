using QuestBooks.Utilities;
using Terraria;
using Terraria.ID;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class GetNinjaSet : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.NinjaHood, ItemID.NinjaShirt, ItemID.NinjaPants);
}
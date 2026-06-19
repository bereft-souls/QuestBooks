using QuestBooks.Utilities;
using Terraria;
using Terraria.ID;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class GetSnowArmor : QBQuest
{
    public override QuestType QuestType => QuestType.Player;
    
    // TODO: Do we want to validate all inventories or just the normal inventory? (e.g. piggy bank, safe, void bag, etc.)
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.EskimoHood, ItemID.EskimoCoat, ItemID.EskimoPants);
}
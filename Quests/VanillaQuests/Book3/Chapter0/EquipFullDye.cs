using QuestBooks.Utilities;
using System.Linq;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class EquipFullDye : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.EnumerateArmorDyes().All(static dye => !dye.Item.IsAir)
        && Main.LocalPlayer.EnumerateAccessoryDyes().All(static dye => !dye.Item.IsAir)
        && Main.LocalPlayer.EnumerateEquipmentDyes().All(static dye => !dye.Item.IsAir);
}
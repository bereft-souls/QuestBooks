using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class GetHMOrePickaxes : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasAnyItem
    (
        ItemID.CobaltPickaxe,
        ItemID.PalladiumPickaxe,
        ItemID.MythrilPickaxe,
        ItemID.OrichalcumPickaxe,
        ItemID.AdamantitePickaxe,
        ItemID.TitaniumPickaxe
    );
}
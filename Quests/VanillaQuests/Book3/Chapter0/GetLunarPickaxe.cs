using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter0;

public class GetLunarPickaxe : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasAnyItem
    (
        ItemID.VortexPickaxe,
        ItemID.VortexDrill,
        ItemID.NebulaPickaxe,
        ItemID.NebulaDrill,
        ItemID.StardustPickaxe,
        ItemID.StardustDrill,
        ItemID.NebulaPickaxe,
        ItemID.NebulaDrill
    );
}
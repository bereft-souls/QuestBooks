using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class GetHMWingMat : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasAnyItem(ItemID.GiantHarpyFeather, ItemID.BoneFeather, ItemID.FireFeather, ItemID.IceFeather);
}
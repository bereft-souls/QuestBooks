using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter3;

public class GetBewitchingTable : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.BewitchingTable);
}
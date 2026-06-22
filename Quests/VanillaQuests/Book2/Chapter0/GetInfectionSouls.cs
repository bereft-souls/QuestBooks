using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class GetInfectionSouls : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasAnyItem(ItemID.SoulofNight, ItemID.SoulofLight);
}
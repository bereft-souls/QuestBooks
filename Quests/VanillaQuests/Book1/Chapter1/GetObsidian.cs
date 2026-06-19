using QuestBooks.Utilities;
using Terraria;
using Terraria.ID;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class GetObsidian : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.Obsidian, 50);
}
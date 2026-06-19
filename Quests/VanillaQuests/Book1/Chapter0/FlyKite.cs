using Terraria;
using Terraria.ID;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class FlyKite : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => ItemID.Sets.IsAKite[Main.LocalPlayer.HeldItem.type];
}
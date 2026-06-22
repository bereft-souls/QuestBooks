using QuestBooks.Content.Sets;
using QuestBooks.Systems.Common.Items;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class GetEveryTrophy : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.GetModPlayer<ItemObtainmentPlayer>().ObtainedAll(ItemSets.Furniture.Trophies);
}
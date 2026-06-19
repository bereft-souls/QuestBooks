using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class SubbiomeAether : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneShimmer;
}
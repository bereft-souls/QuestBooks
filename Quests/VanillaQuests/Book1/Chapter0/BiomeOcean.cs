using QuestBooks.Systems;
using Terraria;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class BiomeOcean : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneBeach;
}
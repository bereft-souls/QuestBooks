using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class BiomeDesert : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneDesert;
}
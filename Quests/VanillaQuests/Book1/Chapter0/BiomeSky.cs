using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class BiomeSky : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneSkyHeight;
}
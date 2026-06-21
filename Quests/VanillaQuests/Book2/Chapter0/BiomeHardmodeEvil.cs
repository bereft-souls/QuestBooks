namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class BiomeHardmodeEvil : QBQuest
{
    public override bool CheckCompletion() => Main.hardMode && (Main.LocalPlayer.ZoneCrimson || Main.LocalPlayer.ZoneCorrupt);
}
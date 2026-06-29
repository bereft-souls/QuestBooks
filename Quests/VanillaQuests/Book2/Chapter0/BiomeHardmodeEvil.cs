namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class BiomeHardmodeEvil : VanillaQuest
{
    public override bool CheckCompletion() => Main.hardMode && (Main.LocalPlayer.ZoneCrimson || Main.LocalPlayer.ZoneCorrupt);
}
namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class SubbiomeUGGlowingMushroom : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneGlowshroom && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight);
}
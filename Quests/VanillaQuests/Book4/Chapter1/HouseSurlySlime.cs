namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseSurlySlime : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.TownSlimeRed);
}
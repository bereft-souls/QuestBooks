namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseZoologist : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.BestiaryGirl);
}